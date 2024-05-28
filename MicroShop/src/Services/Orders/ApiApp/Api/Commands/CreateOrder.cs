using System.Net;
using MicroShop.Services.Orders.ApiApp.Extensions;
using MicroShop.Services.Orders.ApiApp.Infrastructure.Data;
using MicroShop.Services.Orders.ApiApp.Models;
using MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Requests;
using MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MicroShop.Services.Orders.ApiApp.Api.Commands;

internal sealed class CreateOrder
{
    public static async Task<IResult> Handle([FromBody] CreateOrderDto model, HttpContext httpContext, [FromServices] AppDbContext dbContext, [FromServices] ILogger<CreateOrder> logger)
    {
        await Task.Yield();

        if (!httpContext.TryGetUserIdFromHttpContext(out string? userId))
        {
            return TypedResults.BadRequest(ResponseDto.Error<OrderSummaryDto>("not authorized"));
        }

        try
        {
            OrderHeader order = model.ToOrderHeader(userId);
            dbContext.OrderHeaders.Add(order);
            await dbContext.SaveChangesAsync();
            return TypedResults.Ok(ResponseDto.Ok(order.ToSummary()));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unable to save order");
            return new StatusCodeWithResponseResult(HttpStatusCode.InternalServerError, ResponseDto.Error<OrderSummaryDto>(ex.Message));
        }
    }
}
