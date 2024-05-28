
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace MicroShop.Services.ShoppingCart.ApiApp.Infrastructure.Auth;

public sealed class ApiAuthenticationHttpClientHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    private const string TokenName = "access_token";
    private const string TokenType = "Bearer";

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {

        HttpContext? httpContext = httpContextAccessor.HttpContext;
        if (httpContext is not null && await httpContext.GetTokenAsync(TokenName) is { } token)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(TokenType, token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
