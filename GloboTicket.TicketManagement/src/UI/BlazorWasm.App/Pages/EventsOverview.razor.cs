using GloboTicket.TicketManagement.UI.ApiClient.Contracts;
using GloboTicket.TicketManagement.UI.ApiClient.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GloboTicket.TicketManagement.UI.BlazorWasm.App.Pages;

public partial class EventsOverview
{
    [Inject]
    public IEventDataService EventDataService { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    public ICollection<EventListViewModel>? Events { get; set; } 

    [Inject]
    public IJSRuntime JsRuntime { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        Events = (await EventDataService.GetEventsPage(1, int.MaxValue, default)).Items.ToList();
    }

    protected void AddNewEvent()
    {
        NavigationManager.NavigateTo("/eventdetails");
    }

    [Inject]
    public HttpClient HttpClient { get; set; } = default!;

    protected async Task ExportEvents()
    {
        if (await JsRuntime.InvokeAsync<bool>("confirm", $"Do you want to export this list to Excel?"))
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"https://localhost:7020/api/events/export");
            response.EnsureSuccessStatusCode();
            byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
            string fileName = $"MyReport{DateTime.Now.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)}.csv";
            await JsRuntime.InvokeAsync<object>("saveAsFile", fileName, Convert.ToBase64String(fileBytes));
        }
    }
}
