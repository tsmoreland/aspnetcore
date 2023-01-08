using Blazored.LocalStorage;
using GloboTicket.TicketManagement.UI.ApiClient.Services;
using GloboTicket.TicketManagement.UI.BlazorWasm.App.Auth;
using GloboTicket.TicketManagement.UI.BlazorWasm.App.Contracts;
using Microsoft.AspNetCore.Components.Authorization;

namespace GloboTicket.TicketManagement.UI.BlazorWasm.App.Services;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly IClient _client;
    private readonly ILocalStorageService _localStorageService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthenticationService(IClient client, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
        _authenticationStateProvider = authenticationStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
    }

    /// <inheritdoc />
    public async ValueTask<bool> Authenticate(string email, string password)
    {
        await Task.CompletedTask;
        return false;
        /*
        try
        {
            AuthenticationRequest authenticationRequest = new AuthenticationRequest() { Email = email, Password = password };
            var authenticationResponse = await _client.AuthenticateAsync(authenticationRequest);

            if (authenticationResponse.Token != string.Empty && _authenticationStateProvider is BearerTokenInLocalStorageAuthenticationStateProvider stateProvider)
            {
                await _localStorageService.SetItemAsync("token", authenticationResponse.Token);
                stateProvider.SetUserAuthenticated(email);
                _client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authenticationResponse.Token);
                return true;
            }
            return false;
        }
        catch 
        {
            return false;
        }
        */
    }

    /// <inheritdoc />
    public async ValueTask<bool> Register(string firstName, string lastName, string userName, string email, string password)
    {
        await Task.CompletedTask;
        return false;

        /*
        RegistrationRequest registrationRequest = new RegistrationRequest() { FirstName = firstName, LastName = lastName, Email = email, UserName = userName, Password = password };
        var response = await _client.RegisterAsync(registrationRequest);

        return response.UserId is not { Length: > 0 };
        */
    }

    /// <inheritdoc />
    public async ValueTask Logout()
    {
        // not entirely convinced by this approach, it works but I think a more explicit request to IdP would be better - in addition to this
        await _localStorageService.RemoveItemAsync("token");
        if (_authenticationStateProvider is BearerTokenInLocalStorageAuthenticationStateProvider stateProvider)
        {
            stateProvider.SetUserLoggedOut();
        }
        _client.HttpClient.DefaultRequestHeaders.Authorization = null;
    }
}
