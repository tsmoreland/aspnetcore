using System.Diagnostics.CodeAnalysis;
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

        static async Task<Results<Created<ResponseDto<CartSummaryDto>>, BadRequest<ResponseDto<CartSummaryDto>>>> Handler([FromBody] UpsertCartDto model, [FromServices] HttpContext context, [FromServices] ICartService cartService)
        {
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
            Handler([FromServices] HttpContext context, [FromServices] ICartService cartService)
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
            Handler([FromRoute] int cartHeaderId, [FromBody] string couponCode, [FromServices] HttpContext context, [FromServices] ICartService cartService)
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
            .WithName("ApplyCoupon")
            .WithOpenApi();
        return builder;

        static async Task<Results<Ok<ResponseDto>, NotFound<ResponseDto>, BadRequest<ResponseDto>>>
            Handler([FromRoute] int cartHeaderId, [FromServices] HttpContext context, [FromServices] ICartService cartService)
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

        static async Task<Results<NoContent, BadRequest<ResponseDto>>> Handler([FromRoute] int id, [FromServices] HttpContext context, [FromServices] ICartService cartService)
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
        Claim? claim = context.User.Claims.FirstOrDefault(c => c.Subject?.Name == JwtRegisteredClaimNames.Sub);
        userId = claim?.Value;
        return userId is not null;
    }
}
