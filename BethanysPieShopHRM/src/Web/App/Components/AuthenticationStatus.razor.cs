using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;

namespace BethanysPieShopHRM.Web.App.Components;

public partial class AuthenticationStatus
{
    [Inject]
    public NavigationManager Navigation { get; set; } = default!;

    [Inject]
    public JSRuntime JavaScript { get; set; } = default!;

    private void BeginSignOut(MouseEventArgs e)
    {
        _ = e;
        Navigation.NavigateToLogout("authentication/logout");
    }

    private async Task ConfirmLogout(LocationChangingContext context)
    {
        // prompt if the user is sure via a quick show pop up like component
        bool confirmed = await JavaScript.InvokeAsync<bool>("window.confirm", "Are you sure?");
        if (!confirmed)
        {
            context.PreventNavigation();
        }
    }
}
