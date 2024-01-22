using System.Net.Http.Headers;
using GloboTicket.TicketManagement.UI.ApiClient.Contracts;
using GloboTicket.TicketManagement.UI.ApiClient.Services;
using GloboTicket.TicketManagement.UI.BlazorWasm.App.Auth;
using GloboTicket.TicketManagement.UI.BlazorWasm.App.Contracts;
using Microsoft.AspNetCore.Components.Authorization;

namespace GloboTicket.TicketManagement.UI.BlazorWasm.App.Services;

public sealed class AuthenticationService(IClient client, ITokenRepository tokenRepository, AuthenticationStateProvider authenticationStateProvider) : IAuthenticationService
{
    private readonly IClient _client = client ?? throw new ArgumentNullException(nameof(client));
    private readonly ITokenRepository _tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
    private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));

    /// <inheritdoc />
    public async ValueTask<bool> Authenticate(string email, string password)
    {
        try
        {
            AuthenticationRequest authenticationRequest = new() { Email = email, Password = password };
            AuthenticationResponse authenticationResponse = await _client.AuthenticateAsync(authenticationRequest);

            if (authenticationResponse.Token == string.Empty ||
                _authenticationStateProvider is not BearerTokenInLocalStorageAuthenticationStateProvider stateProvider)
            {
                return false;
            }

            await _tokenRepository.AddOrSetTokenAsync("token", authenticationResponse.Token!, default);
            stateProvider.SetUserAuthenticated(email);
            _client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authenticationResponse.Token);
            return true;
        }
        catch 
        {
            return false;
        }
    }

    /// <inheritdoc />
    public async ValueTask<bool> Register(string firstName, string lastName, string userName, string email, string password)
    {
        RegistrationRequest registrationRequest = new() { FirstName = firstName, LastName = lastName, Email = email, UserName = userName, Password = password };
        RegistrationResponse response = await _client.RegisterAsync(registrationRequest);
        return response.UserId != Guid.Empty;
    }

    /// <inheritdoc />
    public async ValueTask Logout()
    {
        // not entirely convinced by this approach, it works but I think a more explicit request to IdP would be better - in addition to this
        await _tokenRepository.RemoveTokenAsync("token", default);
        if (_authenticationStateProvider is BearerTokenInLocalStorageAuthenticationStateProvider stateProvider)
        {
            stateProvider.SetUserLoggedOut();
        }
        _client.HttpClient.DefaultRequestHeaders.Authorization = null;
    }
}
