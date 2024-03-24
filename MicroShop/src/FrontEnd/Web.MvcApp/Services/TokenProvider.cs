using MicroShop.Web.MvcApp.Services.Contracts;

namespace MicroShop.Web.MvcApp.Services;

public sealed class TokenProvider(IHttpContextAccessor contextAccessor) : ITokenProvider
{
    private const string CookieName = "MicroShop.Session.ChocolateChip";

    /// <inheritdoc />
    public void SetToken(string token)
    {
        contextAccessor.HttpContext?.Response.Cookies.Append(CookieName, token);
    }

    /// <inheritdoc />
    public string? GetToken()
    {
        return contextAccessor.HttpContext?.Request.Cookies.TryGetValue(CookieName, out string? token) is true
            ? token
            : null;
    }

    /// <inheritdoc />
    public void ClearToken()
    {
        contextAccessor.HttpContext?.Response.Cookies.Delete(CookieName);
    }
}
