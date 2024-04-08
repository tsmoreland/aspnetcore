﻿using MicroShop.Services.ShoppingCart.ApiApp.Services.Contracts;

namespace MicroShop.Services.ShoppingCart.ApiApp.Services;

public sealed class TokenProvider(IHttpContextAccessor contextAccessor) : ITokenProvider
{
    /// <inheritdoc />
    public string? GetBearerToken()
    {
        return contextAccessor.HttpContext?.Request.Headers.Authorization is { } value
            ? value.ToString()
            : null;
    }
}
