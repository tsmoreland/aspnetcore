using GlobalTicket.TicketManagement.UI.BlazorWasm.App.Auth;
using GlobalTicket.TicketManagement.UI.BlazorWasm.App.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GlobalTicket.TicketManagement.UI.BlazorWasm.App.Pages;

public partial class Index
{
    [Inject]
    private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public IAuthenticationService AuthenticationService { get; set; } = default!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await ((BearerTokenInLocalStorageAuthenticationStateProvider)AuthenticationStateProvider).GetAuthenticationStateAsync();
    }

    protected void NavigateToLogin()
    {
        NavigationManager.NavigateTo("login");
    }

    protected void NavigateToRegister()
    {
        NavigationManager.NavigateTo("register");
    }

    protected async void Logout()
    {
        await AuthenticationService.Logout();
    }
}
