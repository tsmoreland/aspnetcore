using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace FlightPlan.Api.App.Authentication;

/// <inheritdoc />
public sealed class BasicAuthenticationHandler(
    IUserService userService,
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger, UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private readonly IUserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));

    /// <inheritdoc />
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        User? user;
        try
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                Logger.LogError("missing Authorization header");
                return AuthenticateResult.Fail("authorization header not provided");
            }

            var authHeader = AuthenticationHeaderValue.Parse(authorizationHeader.ToString());
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? string.Empty);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);

            var username = credentials[0];
            var password = credentials[1];

            user = await _userService.Authenticate(username, password).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "an error occurred attempting to authentiation user");
            return AuthenticateResult.Fail("Invalid username or password");
        }

        if (user is null)
        {
            return AuthenticateResult.Fail("Invalid username or password");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        ClaimsIdentity identity = new(claims, Scheme.Name);
        ClaimsPrincipal principal = new(identity);
        AuthenticationTicket ticket = new(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
