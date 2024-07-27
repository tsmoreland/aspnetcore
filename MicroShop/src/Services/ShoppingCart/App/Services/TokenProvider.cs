using MicroShop.Services.ShoppingCart.App.Services.Contracts;

namespace MicroShop.Services.ShoppingCart.App.Services;

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
