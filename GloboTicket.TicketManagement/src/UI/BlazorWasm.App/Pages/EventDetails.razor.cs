using System.Collections.ObjectModel;
using GloboTicket.TicketManagement.UI.ApiClient.Contracts;
using GloboTicket.TicketManagement.UI.ApiClient.Services;
using GloboTicket.TicketManagement.UI.ApiClient.ViewModels;
using Microsoft.AspNetCore.Components;
using EventDetailViewModel = GloboTicket.TicketManagement.UI.ApiClient.ViewModels.EventDetailViewModel;

namespace GloboTicket.TicketManagement.UI.BlazorWasm.App.Pages;

public partial class EventDetails
{
    [Inject]
    public IEventDataService EventDataService { get; set; } = default!;

    [Inject]
    public ICategoryDataService CategoryDataService { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    public EventDetailViewModel EventDetailViewModel { get; set; } = new() { Date = DateTime.Now.AddDays(1) };

    public ObservableCollection<CategoryViewModel> Categories { get; set; } = new();

    public string? Message { get; set; }
    public string? SelectedCategoryId { get; set; }

    [Parameter]
    public string EventId { get; set; } = string.Empty;
    private Guid _selectedEventId = Guid.Empty;

    protected override async Task OnInitializedAsync()
    {
        if (Guid.TryParse(EventId, out _selectedEventId))
        {
            EventDetailViewModel = (await EventDataService.GetEventById(_selectedEventId, default))!;
            SelectedCategoryId = EventDetailViewModel.CategoryId.ToString();
        }

        List<CategoryViewModel> list = (await CategoryDataService.GetCategoriesPage(1, int.MaxValue, default))
            .Items
            .ToList();
        Categories = new ObservableCollection<CategoryViewModel>(list);
        SelectedCategoryId = Categories.FirstOrDefault()?.CategoryId.ToString();
    }

    protected async Task HandleValidSubmit()
    {
        EventDetailViewModel.CategoryId = Guid.Parse(SelectedCategoryId ?? string.Empty);
        ApiResponse<Guid> response;

        if (_selectedEventId == Guid.Empty)
        {
            response = await EventDataService.CreateEvent(EventDetailViewModel, default);
        }
        else
        {
            response = await EventDataService.UpdateEvent(EventDetailViewModel, default);
        }
        HandleResponse(response);

    }

    protected async Task DeleteEvent()
    {
        var response = await EventDataService.DeleteEvent(_selectedEventId, default);
        HandleResponse(response);
    }

    protected void NavigateToOverview()
    {
        NavigationManager.NavigateTo("/eventoverview");
    }

    private void HandleResponse(ApiResponse<Guid> response)
    {
        if (response.Success)
        {
            NavigationManager.NavigateTo("/eventoverview");
        }
        else
        {
            Message = response.Message;
            if (response.ProblemDetails == null)
            {
                return;
            }

            foreach (string problme in response.ProblemDetails.Values)
            {
                Message += problme;
            }
        }
    }
}
