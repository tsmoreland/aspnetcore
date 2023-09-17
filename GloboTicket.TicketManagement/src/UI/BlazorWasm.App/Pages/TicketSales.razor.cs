using GloboTicket.TicketManagement.UI.ApiClient.Contracts;
using GloboTicket.TicketManagement.UI.ApiClient.ViewModels;
using Microsoft.AspNetCore.Components;

namespace GloboTicket.TicketManagement.UI.BlazorWasm.App.Pages;

public partial class TicketSales
{
    private int? _pageNumber = 1;

    [Inject]
    public IOrderDataService OrderDataService { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    public string SelectedMonth { get; set; } = default!;
    public string SelectedYear { get; set; } = default!;

    public List<string> MonthList { get; set; } = new List<string>() { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
    public List<string> YearList { get; set; } = new List<string>() { "2020", "2021", "2022" };


    private Page<OrdersForMonthListViewModel>? PaginatedList { get; set; }

    private IEnumerable<OrdersForMonthListViewModel>? _ordersList;

    protected async Task GetSales()
    {
        DateTime dt = new(int.Parse(SelectedYear), int.Parse(SelectedMonth), 1);

        Page<OrdersForMonthListViewModel> orders = await OrderDataService.GetPagedOrderForMonth(_pageNumber!.Value, 5, dt, default);
        PaginatedList = orders;
        _ordersList = PaginatedList.Items;

        StateHasChanged();
    }

    public async void PageIndexChanged(int newPageNumber)
    {
        if (newPageNumber < 1 || newPageNumber > PaginatedList?.TotalPages)
        {
            return;
        }
        _pageNumber = newPageNumber;
        await GetSales();
        StateHasChanged();
    }
}
