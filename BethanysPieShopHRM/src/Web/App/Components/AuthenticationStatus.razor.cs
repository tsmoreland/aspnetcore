using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BethanysPieShopHRM.Web.App.Components;

public partial class AuthenticationStatus
{
    [Inject]
    public NavigationManager Navigation { get; set; } = default!;

    [Inject]
    public SignOutSessionStateManager SessionManager { get; set; } = default!;

    private async Task BeginSignOut(MouseEventArgs e)
    {
        _ = e;
        await SessionManager.SetSignOutState();
        Navigation.NavigateTo("authentication/login");
    }
}
