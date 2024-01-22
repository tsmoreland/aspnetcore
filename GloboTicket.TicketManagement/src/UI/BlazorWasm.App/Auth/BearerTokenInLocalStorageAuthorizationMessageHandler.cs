using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace GloboTicket.TicketManagement.UI.BlazorWasm.App.Auth;

public class BearerTokenInLocalStorageAuthorizationMessageHandler(ILocalStorageService localStorageService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string? token = await localStorageService.GetItemAsync<string>("token", cancellationToken);
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
