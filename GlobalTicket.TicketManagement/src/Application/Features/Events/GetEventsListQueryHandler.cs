using AutoMapper;
using GlobalTicket.TicketManagement.Application.Contracts.Persistence;
using GlobalTicket.TicketManagement.Application.Features.Events.Specifications;
using GlobalTicket.TicketManagement.Domain.Common;
using GlobalTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GlobalTicket.TicketManagement.Application.Features.Events;

public sealed class GetEventsListQueryHandler : IRequestHandler<GetEventsListQuery, Page<EventViewModel>>
{
    private readonly IMapper _mapper;
    private readonly IAsyncRepository<Event> _eventRepository;

    public GetEventsListQueryHandler(IMapper mapper, IAsyncRepository<Event> eventRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
    }

    /// <inheritdoc />
    public async Task<Page<EventViewModel>> Handle(GetEventsListQuery request, CancellationToken cancellationToken)
    {
        return (await _eventRepository.GetPage(request.PageRequest, orderBy: new OrderByByDateSpecification(), cancellationToken: cancellationToken))
            .Map<Event, EventViewModel>(_mapper);
    }
}
