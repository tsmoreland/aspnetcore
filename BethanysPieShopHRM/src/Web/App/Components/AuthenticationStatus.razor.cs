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
    public IJSRuntime JavaScript { get; set; } = default!;

    private void BeginSignOut(MouseEventArgs e)
    {
        _ = e;
        Navigation.NavigateToLogout("authentication/logout");
    }

    private async Task ConfirmLogout(LocationChangingContext context)
    {
        if (context.TargetLocation != "authentication/logout")
        {
            return;
        }

        // prompt if the user is sure via a quick show pop up like component
        var confirmed = await JavaScript.InvokeAsync<bool>("window.confirm", "Are you sure?").ConfigureAwait(false);
        if (!confirmed)
        {
            context.PreventNavigation();
        }
    }
}
