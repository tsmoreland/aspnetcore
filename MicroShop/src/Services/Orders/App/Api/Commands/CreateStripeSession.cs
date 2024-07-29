using System.Net;
using MicroShop.Services.Orders.App.Extensions;
using MicroShop.Services.Orders.App.Infrastructure.Data;
using MicroShop.Services.Orders.App.Models;
using MicroShop.Services.Orders.App.Models.DataTransferObjects.Notifications;
using MicroShop.Services.Orders.App.Models.DataTransferObjects.Requests;
using MicroShop.Services.Orders.App.Models.DataTransferObjects.Responses;
using MicroShop.Services.Orders.App.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe.Checkout;

namespace MicroShop.Services.Orders.App.Api.Commands;

internal sealed class CreateStripeSession
{
    private static class SessionModes
    {
        public const string Payment = "payment";
    }


    public static async Task<Results<Ok<ResponseDto<StripeResponseDto>>, BadRequest<ResponseDto<StripeResponseDto>>, StatusCodeWithResponseResult<StripeResponseDto>>>
        Handle([FromBody] StripeRequestDto request, HttpContext httpContext, [FromServices] AppDbContext dbContext, [FromServices] IRabbitMessageSender messageSender, [FromServices] IOptions<RabbitExchange> exchangeOptions)
    {
        try
        {
            if (!httpContext.TryGetUserIdFromHttpContext(out string? userId))
            {
                return new StatusCodeWithResponseResult<StripeResponseDto>(HttpStatusCode.Unauthorized, ResponseDto.Error<StripeResponseDto>("Not authorized"));
            }

            // Consider the use of mediator to fire this off to somewhere else
            if (!OrderHeader.CanUpdateStatus(OrderStatus.Approved, request.Order.Status))
            {
                return TypedResults.BadRequest(ResponseDto.Error<StripeResponseDto>($"Unable to processing payment for order with Status {request.Order.Status}"));
            }

            int reardsValue = (int)ConvertToPenceOrCents(request.Order.OrderTotal) / 100;
            List<SessionLineItemOptions> lineItems = request.Order.Details
                .Select(i => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = ConvertToPenceOrCents(i.Price),
                        Currency = "gbp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = i.ProductName,
                        }
                    },
                    Quantity = i.Count,
                })
                .ToList();


            SessionCreateOptions options = new()
            {
                SuccessUrl = request.ApprovedUrl.ToString(),
                CancelUrl = request.CancelUrl.ToString(),
                LineItems = lineItems,
                Mode = SessionModes.Payment,
            };

            if (request.Order.Discount > 0.0)
            {
                options.Discounts = [new SessionDiscountOptions { Coupon = request.Order.CouponCode, }];
            }

            SessionService sessionService = new();
            Session session = await sessionService.CreateAsync(options);

            ResponseDto dbResult = await UpdateOrderWithStripeSessionId(request.Order.Id, session.Id, session.PaymentIntentId, OrderStatus.Approved, dbContext);
            if (!dbResult.Success)
            {
                return new StatusCodeWithResponseResult<StripeResponseDto>(HttpStatusCode.InternalServerError, ResponseDto.Error<StripeResponseDto>(dbResult.ErrorMessage ?? "Unknown"));
            }

            RewardsDto dto = new(userId, reardsValue, request.Order.Id);
            messageSender.SendMessage(dto, exchangeOptions.Value);

            return TypedResults.Ok(ResponseDto.Ok(new StripeResponseDto(session.Url)));
        }
        catch (Exception ex)
        {
            return new StatusCodeWithResponseResult<StripeResponseDto>(HttpStatusCode.InternalServerError, ResponseDto.Error<StripeResponseDto>(ex.Message));
        }

        static long ConvertToPenceOrCents(double price) => (long)price * 100; // 100 pence in a pound or cents in a dollar
    }

    private static async Task<ResponseDto> UpdateOrderWithStripeSessionId(int orderId, string stripeSessionId, string paymentIntentId, OrderStatus status, AppDbContext dbContext)
    {
        try
        {
            await dbContext.OrderHeaders
                .Where(i => i.Id == orderId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.StripeSessionId, stripeSessionId)
                    .SetProperty(p => p.Status, status)
                    .SetProperty(p => p.PaymentIntentId, paymentIntentId));
            return ResponseDto.Ok();
        }
        catch (Exception ex)
        {
            return ResponseDto.Error(ex.Message);
        }
    }
}
