using System.Security.Claims;
using MediatR;
using MicroShop.Services.Orders.ApiApp.Api.Commands;
using MicroShop.Services.Orders.ApiApp.Extensions;
using MicroShop.Services.Orders.ApiApp.Features.Commands.UpdateOrderStatus;
using MicroShop.Services.Orders.ApiApp.Features.Queries.GetOrderDetailsById;
using MicroShop.Services.Orders.ApiApp.Features.Queries.GetOrders;
using MicroShop.Services.Orders.ApiApp.Features.Queries.GetOrdersByUserId;
using MicroShop.Services.Orders.ApiApp.Models;
using MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Requests;
using MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Stripe;

namespace MicroShop.Services.Orders.ApiApp;

internal static class WebApplicationExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.UseResponseCompression();
        app.UseExceptionHandler();
        app.UseRateLimiter();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        StripeConfiguration.ApiKey = app.Configuration["Stripe::SecretKey"];

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();

        return app.UseMinimalApi();
    }

    private static WebApplication UseMinimalApi(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("api/orders");

        group
            .MapPost("", CreateOrder.Handle)
            .RequireAuthorization()
            .WithOpenApi()
            .WithName(nameof(CreateOrder));

        group
            .MapPost("stripe", CreateStripeSession.Handle)
            .RequireAuthorization()
            .WithOpenApi()
            .WithName(nameof(CreateStripeSession));

        group
            .MapGet("{orderId:int}/summary", async Task<Results<Ok<ResponseDto<OrderSummaryDto>>, NotFound<ResponseDto<OrderSummaryDto>>>>
                ([FromRoute] int orderId, HttpContext httpContext, [FromServices] IMediator mediator) =>
                {
                    ClaimsIdentity? identity = httpContext.User.Identities.FirstOrDefault();
                    string? role = identity?.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
                    string? userId = null;
                    if (role is not "ADMIN")
                    {
                        if (!httpContext.TryGetUserIdFromHttpContext(out userId))
                        {
                            // Replace with 401 eventually
                            return TypedResults.NotFound(ResponseDto.Error<OrderSummaryDto>("order not found"));
                        }
                    }

                    OrderHeader? order = await mediator.Send(new GetOrderDetailsByIdRequest(orderId, userId));
                    return order is not null
                        ? TypedResults.Ok(ResponseDto.Ok(new OrderSummaryDto(order)))
                        : TypedResults.NotFound(ResponseDto.Error<OrderSummaryDto>("order not found"));
                })
            .RequireAuthorization()
            .WithName("GetByOrderId")
            .WithOpenApi();

        group
            .MapGet("{userId}", ([FromRoute] string userId, [FromServices] IMediator mediator) =>
            {
                IAsyncEnumerable<OrderStatusDto> data = mediator.CreateStream(new GetOrdersByUserIdRequest(userId))
                    .Select(o => new OrderStatusDto(o));
                return Results.Ok(ResponseDto.Ok(data));
            })
            .RequireAuthorization("ADMIN")
            .WithName("GetOrdersByUserId")
            .WithOpenApi();

        group
            .MapGet("", (HttpContext httpContext, [FromServices] IMediator mediator) =>
            {
                ClaimsIdentity? identity = httpContext.User.Identities.FirstOrDefault();
                string? role = identity?.Claims.FirstOrDefault(x => x.Type == "role")?.Value;

                IAsyncEnumerable<OrderStatusDto> data;

                if (role == "ADMIN")
                {
                    data = mediator.CreateStream(new GetOrdersRequest()).Select(o => new OrderStatusDto(o));
                }
                else if (httpContext.TryGetUserIdFromHttpContext(out string? userId))
                {
                    data = mediator.CreateStream(new GetOrdersByUserIdRequest(userId))
                        .Select(o => new OrderStatusDto(o));
                }
                else
                {
                    data = AsyncEnumerable.Empty<OrderStatusDto>();
                }

                return Results.Ok(ResponseDto.Ok(data));

            })
            .RequireAuthorization("ADMIN")
            .WithName("GetOrders")
            .WithOpenApi();

        group
            .MapPut("{orderId:int}/status", async Task<Results<Ok<ResponseDto>, BadRequest<ResponseDto>>> ([FromRoute] int orderId, [FromBody] OrderUpdateStatus status, [FromServices] IMediator mediator) =>
            {
                ResponseDto response = await mediator.Send(new UpdateOrderStatusCommand(orderId, (OrderStatus)status));
                return response.Success
                    ? TypedResults.Ok(response)
                    : TypedResults.BadRequest(response);
            })
            .RequireAuthorization("ADMIN")
            .WithName("UpdateOrderStatus")
            .WithOpenApi();

        return app;
    }
}
