using GloboTicket.TicketManagement.UI.ApiClient.ViewModels;

namespace GloboTicket.TicketManagement.UI.ApiClient.Contracts;

public interface IEventDataService
{
    ValueTask<Page<EventListViewModel>> GetEventsPage(int pageNumber, int pageSize, CancellationToken cancellationToken);
    ValueTask<EventDetailViewModel?> GetEventById(Guid id, CancellationToken cancellationToken);
    ValueTask<Services.ApiResponse<Guid>> CreateEvent(EventDetailViewModel eventDetailViewModel, CancellationToken cancellationToken);
    ValueTask<Services.ApiResponse<Guid>> UpdateEvent(EventDetailViewModel eventDetailViewModel, CancellationToken cancellationToken);
    ValueTask<Services.ApiResponse<Guid>> DeleteEvent(Guid id, CancellationToken cancellationToken);
}
