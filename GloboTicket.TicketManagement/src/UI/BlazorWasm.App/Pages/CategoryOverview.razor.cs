using GloboTicket.TicketManagement.UI.ApiClient.Contracts;
using GloboTicket.TicketManagement.UI.ApiClient.ViewModels;
using Microsoft.AspNetCore.Components;

namespace GloboTicket.TicketManagement.UI.BlazorWasm.App.Pages;

public partial class CategoryOverview
{
    [Inject]
    public ICategoryDataService CategoryDataService { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    public ICollection<CategoryEventsViewModel> Categories { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        Categories = (await CategoryDataService.GetCategoriesWithEventsPage(1, int.MaxValue, false, default)).Items.ToList();
    }

    protected async void OnIncludeHistoryChanged(ChangeEventArgs args)
    {
        if (args.Value is true)
        {
            Categories = (await CategoryDataService.GetCategoriesWithEventsPage(1, int.MaxValue, true, default))
                .Items
                .ToList();
        }
        else
        {
            Categories = (await CategoryDataService.GetCategoriesWithEventsPage(1, int.MaxValue, false, default))
                .Items
                .ToList();
        }
    }
}
