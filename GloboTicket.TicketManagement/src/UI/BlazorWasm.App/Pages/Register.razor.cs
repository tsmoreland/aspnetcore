using GloboTicket.TicketManagement.UI.ApiClient.ViewModels;
using GloboTicket.TicketManagement.UI.BlazorWasm.App.Contracts;
using Microsoft.AspNetCore.Components;

namespace GloboTicket.TicketManagement.UI.BlazorWasm.App.Pages;

public partial class Register
{
    public RegisterViewModel RegisterViewModel { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    public string Message { get; set; } = string.Empty;

    [Inject]
    private IAuthenticationService AuthenticationService { get; set; } = default!;

    public Register()
    {
    }

    protected override void OnInitialized()
    {
        RegisterViewModel = new RegisterViewModel();
    }

    protected async void HandleValidSubmit()
    {
        var result = await AuthenticationService.Register(RegisterViewModel.FirstName, RegisterViewModel.LastName, RegisterViewModel.UserName, RegisterViewModel.Email, RegisterViewModel.Password);

        if (result)
        {
            NavigationManager.NavigateTo("home");
        }
        Message = "Something went wrong, please try again.";
    }
}
