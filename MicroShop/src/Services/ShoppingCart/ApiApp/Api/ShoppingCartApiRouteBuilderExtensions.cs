using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects;
using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects.Request;
using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects.Response;
using MicroShop.Services.ShoppingCart.ApiApp.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MicroShop.Services.ShoppingCart.ApiApp.Api;

internal static class ShoppingCartApiRouteBuilderExtensions  
{
    public static RouteGroupBuilder MapShoppingCartApi(this RouteGroupBuilder builder)
    {
        return builder
            .MapUpsertCart()
            .MapGetCartByUserId()
            .MapApplyCoupon()
            .MapRemoveCoupon()
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

        static async Task<Results<Created<ResponseDto<CartSummaryDto>>, BadRequest<ResponseDto<CartSummaryDto>>>> Handler([FromBody] UpsertCartDto model, HttpContext context, [FromServices] ICartService cartService)
        {
            string? token = await context.GetTokenAsync("access_token");
            if (token is not null)
            {
            }

            if (!TryGetUserIdFromHttpContext(context, out string? userId))
            {
                return TypedResults.BadRequest(ResponseDto.Error<CartSummaryDto>("wrong error response but error is forbidden"));
            }

            ResponseDto<CartSummaryDto> summary = await cartService.Upsert(userId, model);
            return summary.Success
                ? TypedResults.Created("/api/cart", summary)
                : TypedResults.BadRequest(summary);
        }
    }
    private static RouteGroupBuilder MapGetCartByUserId(this RouteGroupBuilder builder)
    {
        builder
            .MapGet("/", Handler)
            .RequireAuthorization()
            .WithName("GetCartByUserId")
            .WithOpenApi();

        return builder;

        static async Task<Results<Ok<ResponseDto<CartSummaryDto>>, NotFound<ResponseDto<CartSummaryDto>>, BadRequest<ResponseDto<CartSummaryDto>>>>
            Handler(HttpContext context, [FromServices] ICartService cartService)
        {
            if (!TryGetUserIdFromHttpContext(context, out string? userId))
            {
                return TypedResults.NotFound(ResponseDto.Error<CartSummaryDto>("no cart found for current user"));
            }

            ResponseDto<CartSummaryDto> summary = await cartService.GetByUserId(userId);
            return summary.Success
                ? TypedResults.Ok(summary)
                : TypedResults.NotFound(summary);
        }
    }
    private static RouteGroupBuilder MapApplyCoupon(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("/{cartHeaderId}/coupon", Handler)
            .WithName("ApplyCoupon")
            .WithOpenApi();

        return builder;

        static async Task<Results<Ok<ResponseDto>, NotFound<ResponseDto>, BadRequest<ResponseDto>>>
            Handler([FromRoute] int cartHeaderId, [FromBody] string couponCode, HttpContext context, [FromServices] ICartService cartService)
        {
            if (!TryGetUserIdFromHttpContext(context, out string? userId))
            {
                return TypedResults.NotFound<ResponseDto>(ResponseDto.Error<ResponseDto>("user not found"));
            }
            if (couponCode is not { Length: > 0 })
            {
                return TypedResults.BadRequest<ResponseDto>(ResponseDto.Error<ResponseDto>("invalid coupon code"));
            }

            ResponseDto result = await cartService.ApplyCoupon(userId, cartHeaderId, couponCode);
            return result.Success
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
    private static RouteGroupBuilder MapRemoveCoupon(this RouteGroupBuilder builder)
    {
        builder
            .MapDelete("/{cartHeaderId}/coupon", Handler)
            .WithName("RemoveCoupon")
            .WithOpenApi();
        return builder;

        static async Task<Results<Ok<ResponseDto>, NotFound<ResponseDto>, BadRequest<ResponseDto>>>
            Handler([FromRoute] int cartHeaderId, HttpContext context, [FromServices] ICartService cartService)
        {
            if (!TryGetUserIdFromHttpContext(context, out string? userId))
            {
                return TypedResults.NotFound(ResponseDto.Error("user not found"));
            }

            ResponseDto result = await cartService.RemoveCoupon(userId, cartHeaderId);
            return result.Success
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }

    private static RouteGroupBuilder MapDeleteCartById(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("/{id}", Handler)
            .RequireAuthorization()
            .WithName("DeleteFromCart")
            .WithOpenApi();

        return builder;

        static async Task<Results<NoContent, BadRequest<ResponseDto>>> Handler([FromRoute] int id, HttpContext context, [FromServices] ICartService cartService)
        {
            if (!TryGetUserIdFromHttpContext(context, out string? userId))
            {
                return TypedResults.BadRequest(ResponseDto.Error("wrong error response but error is forbidden"));
            }

            ResponseDto result = await cartService.RemoveFromCart(userId, id);
            return result.Success
                ? TypedResults.NoContent()
                : TypedResults.BadRequest(result);
        }
    }

    private static bool TryGetUserIdFromHttpContext(HttpContext context, [NotNullWhen(true)] out string? userId)
    {
        const string nameIdentiferClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        Claim? claim = context.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub) ?? context.User.Claims.FirstOrDefault(c => c.Type == nameIdentiferClaim);
        userId = claim?.Value;
        return userId is not null;
    }
}
