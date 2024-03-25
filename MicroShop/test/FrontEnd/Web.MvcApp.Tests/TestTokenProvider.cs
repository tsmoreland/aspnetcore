using MicroShop.Web.MvcApp.Services.Contracts;

namespace MicroShop.Web.MvcApp.Tests;

internal sealed class TestTokenProvider : ITokenProvider
{
    private volatile string? _token = null;

    /// <inheritdoc />
    public void SetToken(string token) => _token = token;

    /// <inheritdoc />
    public string? GetToken() => _token;

    /// <inheritdoc />
    public void ClearToken() => _token = null;
}
