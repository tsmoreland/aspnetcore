using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MicroShop.Services.ShoppingCart.App.Models.DataTransferObjects;
using MicroShop.Services.ShoppingCart.App.Models.DataTransferObjects.Request;
using MicroShop.Services.ShoppingCart.App.Models.DataTransferObjects.Response;
using MicroShop.Services.ShoppingCart.App.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MicroShop.Services.ShoppingCart.App.Api;

internal static class ShoppingCartApiRouteBuilderExtensions
{
    public static RouteGroupBuilder MapShoppingCartApi(this RouteGroupBuilder builder)
    {
        return builder
            .MapUpsertCart()
            .MapGetCartByUserId()
            .MapEmailCart()
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
    private static RouteGroupBuilder MapEmailCart(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("/email", Handler)
            .RequireAuthorization()
            .WithName("EmailCart")
            .WithOpenApi();
        return builder;

        static async Task<Results<Ok<ResponseDto>, NotFound<ResponseDto>, BadRequest<ResponseDto>>> Handler(HttpContext context, [FromServices] ICartService cartService)
        {
            if (!TryGetUserDetailsFromHttpContext(context, out string? userId, out string? name, out string? emailAddress))
            {
                return TypedResults.NotFound<ResponseDto>(ResponseDto.Error<ResponseDto>("user not found"));
            }

            ResponseDto result = await cartService.EmailCart(userId, name, emailAddress);
            return result.Success
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);

        }
    }
    private static RouteGroupBuilder MapApplyCoupon(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("/{cartHeaderId:int}/coupon", Handler)
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
            .MapDelete("/{cartHeaderId:int}/coupon", Handler)
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
            .MapDelete("/{id:int}", Handler)
            .RequireAuthorization()
            .WithName("DeleteFromCart")
            .WithOpenApi();

        return builder;

        static async Task<Results<NoContentWithResponseResult, BadRequest<ResponseDto>>> Handler([FromRoute] int id, HttpContext context, [FromServices] ICartService cartService)
        {
            if (!TryGetUserIdFromHttpContext(context, out string? userId))
            {
                return TypedResults.BadRequest(ResponseDto.Error("wrong error response but error is forbidden"));
            }

            ResponseDto result = await cartService.RemoveFromCart(userId, id);
            return result.Success
                ? NoContentWithResponseResult.Success()
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

    private static bool TryGetUserDetailsFromHttpContext(HttpContext context, [NotNullWhen(true)] out string? userId, [NotNullWhen(true)] out string? name, [NotNullWhen(true)] out string? emailAddress)
    {
        userId = null;
        name = null;
        emailAddress = null;

        const string nameIdentiferClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        const string emailClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";

        string[] requiredClaims = [nameIdentiferClaim, emailClaim, JwtRegisteredClaimNames.Name];

        IEnumerable<Claim> claims = context.User.Claims.Where(c => requiredClaims.Contains(c.Type));
        foreach (Claim match in claims)
        {
            switch (match.Type)
            {
                case nameIdentiferClaim:
                    userId = match.Value;
                    break;
                case emailClaim:
                    emailAddress = match.Value;
                    break;
                case JwtRegisteredClaimNames.Name:
                    name = match.Value;
                    break;
            }
        }

        return userId is not null && name is not null && emailAddress is not null;
    }
}
