using System.Security.Claims;
using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects;
using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects.Request;
using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects.Response;
using MicroShop.Services.ShoppingCart.ApiApp.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace MicroShop.Services.ShoppingCart.ApiApp.Api;

internal static class ShoppingCartApiRouteBuilderExtensions  
{
    public static RouteGroupBuilder MapShoppingCartApi(this RouteGroupBuilder builder)
    {
        return builder
            .MapUpsertCart()
            .MapGetCartById()
            .MapGetAllCarts()
            .MapUpdateCartById()
            .MapDeleteCartById();
    }

    private static RouteGroupBuilder MapUpsertCart(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("/", Handler)
            .RequireAuthorization()
            .WithName("UpsertCart")
            .WithOpenApi();
        return builder;

        static async Task<Results<Ok<ResponseDto<CartSummaryDto>>, BadRequest<ResponseDto<CartSummaryDto>>>> Handler([FromBody] UpsertCartDto model, [FromServices] HttpContext context, [FromServices] ICartService cartService)
        {
            Claim? claim = context.User.Claims.FirstOrDefault(c => c.Subject?.Name == JwtRegisteredClaimNames.Sub);
            string? userId = claim?.Value;
            if (userId is null)
            {
                return TypedResults.BadRequest(ResponseDto.Error<CartSummaryDto>("wrong error response but error is forbidden"));
            }

            ResponseDto<CartSummaryDto> summary = await cartService.Upsert(userId, model);
            return summary.Success
                ? TypedResults.Ok(summary)
                : TypedResults.BadRequest(summary);
        }
    }
    private static RouteGroupBuilder MapGetCartById(this RouteGroupBuilder builder)
    {
        return builder;
    }
    private static RouteGroupBuilder MapGetAllCarts(this RouteGroupBuilder builder)
    {
        return builder;
    }
    private static RouteGroupBuilder MapUpdateCartById(this RouteGroupBuilder builder)
    {
        return builder;
    }
    private static RouteGroupBuilder MapDeleteCartById(this RouteGroupBuilder builder)
    {
        return builder;
    }
}
