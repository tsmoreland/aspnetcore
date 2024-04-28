using System.Net;
using MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Requests;
using MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using MicroShop.Services.Orders.ApiApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Orders.ApiApp.Api.Commands;

internal sealed class CreateStripeSession
{
    private static class SessionModes
    {
        public const string Payment = "payment";
    }


    public static async Task<Results<Ok<ResponseDto<StripeResponseDto>>, StatusCodeWithResponseResult<StripeResponseDto>>>
        Handle([FromBody] StripeRequestDto request, [FromServices] AppDbContext dbContext)
    {
        try
        {
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

            SessionService sessionService = new();
            Session session = await sessionService.CreateAsync(options);

            // Consider the use of mediator to fire this off to somewhere else
            ResponseDto dbResult = await UpdateOrderWithStripeSessionId(request.Order.Id, session.Id, dbContext);
            if (!dbResult.Success)
            {
                return new StatusCodeWithResponseResult<StripeResponseDto>(HttpStatusCode.InternalServerError, ResponseDto.Error<StripeResponseDto>(dbResult.ErrorMessage ?? "Unknown"));
            }

            return TypedResults.Ok(ResponseDto.Ok(new StripeResponseDto(session.Url)));
        }
        catch (Exception ex)
        {
            return new StatusCodeWithResponseResult<StripeResponseDto>(HttpStatusCode.InternalServerError, ResponseDto.Error<StripeResponseDto>(ex.Message));
        }

        static long ConvertToPenceOrCents(double price) => (long)price * 100; // 100 pence in a pound or cents in a dollar
    }

    private static async Task<ResponseDto> UpdateOrderWithStripeSessionId(int orderId, string stripeSessionId, AppDbContext dbContext)
    {
        try
        {
            await dbContext.OrderHeaders
                .Where(i => i.Id == orderId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.StripeSessionId, stripeSessionId));
            return ResponseDto.Ok();
        }
        catch (Exception ex)
        {
            return ResponseDto.Error(ex.Message);
        }
    }
}
