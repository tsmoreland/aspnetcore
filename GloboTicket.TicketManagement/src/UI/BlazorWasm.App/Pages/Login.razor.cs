using GloboTicket.TicketManagement.UI.ApiClient.ViewModels;
using GloboTicket.TicketManagement.UI.BlazorWasm.App.Contracts;
using Microsoft.AspNetCore.Components;

namespace GloboTicket.TicketManagement.UI.BlazorWasm.App.Pages;

public partial class Login
{
    public LoginViewModel LoginViewModel { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    public string? Message { get; set; }

    [Inject]
    private IAuthenticationService AuthenticationService { get; set; } = default!;

    public Login()
    {

    }

    protected override void OnInitialized()
    {
        LoginViewModel = new LoginViewModel();
    }

    protected async void HandleValidSubmit()
    {
        if (await AuthenticationService.Authenticate(LoginViewModel.Email, LoginViewModel.Password))
        {
            NavigationManager.NavigateTo("home");
        }
        Message = "Username/password combination unknown";
    }
}
