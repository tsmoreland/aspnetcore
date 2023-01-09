using AutoMapper;
using GloboTicket.TicketManagement.UI.ApiClient.Contracts;
using GloboTicket.TicketManagement.UI.ApiClient.Services;
using GloboTicket.TicketManagement.UI.ApiClient.ViewModels;
using EventDetailViewModel = GloboTicket.TicketManagement.UI.ApiClient.ViewModels.EventDetailViewModel;

namespace GloboTicket.TicketManagement.UI.BlazorWasm.App.Services;

public sealed class EventDataService : BaseDataService, IEventDataService
{
    public EventDataService(IClient client, IMapper mapper, ITokenRepository tokenRepository)
        : base(client, mapper, tokenRepository)
    {
    }

    /// <inheritdoc />
    public async ValueTask<Page<EventListViewModel>> GetEventsPage(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        EventViewModelPage page = await Client.GetEventsAsync(pageNumber, pageSize, cancellationToken);
        return Mapper.Map<Page<EventListViewModel>>(page);
    }

    /// <inheritdoc />
    public async ValueTask<EventDetailViewModel?> GetEventById(Guid id, CancellationToken cancellationToken)
    {
        return Mapper.Map<EventDetailViewModel?>(await Client.GetEventByIdAsync(id, cancellationToken));
    }

    /// <inheritdoc />
    public async ValueTask<ApiResponse<Guid>> CreateEvent(EventDetailViewModel eventDetailViewModel, CancellationToken cancellationToken)
    {
        CreateEventCommand command = Mapper.Map<CreateEventCommand>(eventDetailViewModel);

        try
        {
            EventViewModel response = await Client.AddEventAsync(command, cancellationToken);
            return ApiResponse.Success(response.EventId!.Value);
        }
        catch (ApiException ex)
        {
            return ConvertApiException<Guid>(ex);
        }
        catch (Exception ex)
        {
            return ConvertException<Guid>(ex);
        }
    }

    /// <inheritdoc />
    public async ValueTask<ApiResponse<Guid>> UpdateEvent(EventDetailViewModel eventDetailViewModel, CancellationToken cancellationToken)
    {
        try
        {
            UpdateEventDto dto = Mapper.Map<UpdateEventDto>(eventDetailViewModel);
            Guid id = eventDetailViewModel.EventId;

            await Client.UpdateEventAsync(id, dto, cancellationToken);
            return ApiResponse.Success(id);
        }
        catch (ApiException ex)
        {
            return ConvertApiException<Guid>(ex);
        }
    }

    /// <inheritdoc />
    public async ValueTask<ApiResponse<Guid>> DeleteEvent(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await Client.DeleteEventAsync(id, cancellationToken);
            return ApiResponse.Success(id);
        }
        catch (ApiException ex)
        {
            return ConvertApiException<Guid>(ex);
        }
    }
}
