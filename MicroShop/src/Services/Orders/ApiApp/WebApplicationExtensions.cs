using System.Net;
using MicroShop.Services.Orders.ApiApp.Api;
using MicroShop.Services.Orders.ApiApp.Api.Commands;
using MicroShop.Services.Orders.ApiApp.Api.Queries;
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
            .MapGet("{orderId}", async Task<Results<Ok<ResponseDto<OrderStatusDto>>, NotFound<ResponseDto<OrderStatusDto>>>>
                ([FromRoute] int orderId, [FromServices] GetOrderStatusApiHandler handler) =>
                {
                    OrderHeader? order = await handler.Handle(orderId);

                    return order is not null
                        ? TypedResults.Ok(ResponseDto.Ok(new OrderStatusDto(order)))
                        : TypedResults.NotFound(ResponseDto.Error<OrderStatusDto>("order not found"));
                });
        return app;
    }
}
