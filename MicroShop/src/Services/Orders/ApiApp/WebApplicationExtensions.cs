using System.Net;
using System.Security.Claims;
using Azure;
using MediatR;
using MicroShop.Services.Orders.ApiApp.Api;
using MicroShop.Services.Orders.ApiApp.Api.Commands;
using MicroShop.Services.Orders.ApiApp.Api.Queries;
using MicroShop.Services.Orders.ApiApp.Extensions;
using MicroShop.Services.Orders.ApiApp.Features.Queries.GetOrders;
using MicroShop.Services.Orders.ApiApp.Features.Queries.GetOrdersByUserId;
using MicroShop.Services.Orders.ApiApp.Models;
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
            .MapGet("{orderId:int}", async Task<Results<Ok<ResponseDto<OrderStatusDto>>, NotFound<ResponseDto<OrderStatusDto>>>>
                ([FromRoute] int orderId, [FromServices] GetOrderStatusApiHandler handler) =>
                {
                    OrderHeader? order = await handler.Handle(orderId);

                    return order is not null
                        ? TypedResults.Ok(ResponseDto.Ok(new OrderStatusDto(order)))
                        : TypedResults.NotFound(ResponseDto.Error<OrderStatusDto>("order not found"));
                })
            .RequireAuthorization("ADMIN")
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
                else if (!httpContext.TryGetUserIdFromHttpContext(out string? userId))
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
            .RequireAuthorization()
            .WithName("GetOrders")
            .WithOpenApi();


        return app;
    }
}
