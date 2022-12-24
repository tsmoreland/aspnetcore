using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace GloboTicket.TicketManagement.UI.BlazorWasm.App.Auth;

public sealed class BearerTokenInLocalStorageAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;

    /// <inheritdoc />
    public BearerTokenInLocalStorageAuthenticationStateProvider(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
    }


    /// <inheritdoc />
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string savedToken = await _localStorageService.GetItemAsync<string>("token");
        return string.IsNullOrWhiteSpace(savedToken)
            ? new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))
            : new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseTokenClaims(savedToken), "jwt")));
    }
    public void SetUserAuthenticated(string email)
    {
        ClaimsPrincipal authUser = new(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "apiauth"));
        Task<AuthenticationState> authState = Task.FromResult(new AuthenticationState(authUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void SetUserLoggedOut()
    {
        ClaimsPrincipal anonymousUser = new(new ClaimsIdentity());
        Task<AuthenticationState> authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }

    private static IEnumerable<Claim> ParseTokenClaims(string jwt)
    {
        List<Claim> claims = new();
        string payload = jwt.Split('.')[1];
        byte[] jsonBytes = ParseBase64WithoutPadding(payload);
        Dictionary<string, object?>? keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object?>>(jsonBytes);

        if (keyValuePairs is null)
        {
            return claims;
        }

        if (keyValuePairs.TryGetValue(ClaimTypes.Role, out object? roles) && roles is not null)
        {
            string roleValues = roles?.ToString()?.Trim() ?? string.Empty;
            if (roleValues.StartsWith('['))
            {
                string[]? parsedRoles = JsonSerializer.Deserialize<string[]>(roleValues) ?? Array.Empty<string>();
                claims.AddRange(parsedRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            }
            else if (roleValues is { Length: > 0 })
            {
                claims.Add(new Claim(ClaimTypes.Role, roleValues));
            }


            keyValuePairs.Remove(ClaimTypes.Role);
        }

        IEnumerable<(string Key, string Value)> remainingPairs = keyValuePairs
            .Where(kvp => kvp.Value is not null)
            .Select(kvp => (kvp.Key, kvp.Value!.ToString()!));
        claims.AddRange(remainingPairs.Select(kvp => new Claim(kvp.Key, kvp.Value)));

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }
        return Convert.FromBase64String(base64);
    }
}
