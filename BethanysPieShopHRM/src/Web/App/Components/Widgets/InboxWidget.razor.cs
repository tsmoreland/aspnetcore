using BethanysPieShopHRM.Web.App.Infrastructure;
using Microsoft.AspNetCore.Components;

namespace BethanysPieShopHRM.Web.App.Components.Widgets;

public partial class InboxWidget
{
    [Inject]
    public ApplicationState ApplicationState { get; set; } = default!;

    public int MessageCount { get; set; }

    protected override void OnInitialized()
    {
        MessageCount = ApplicationState.NumberOfMessages;
    }
}
