using AutoMapper;
using GlobalTicket.TicketManagement.Application.Contracts.Persistence;
using GlobalTicket.TicketManagement.Application.Contracts.Persistence.Specifications;
using GlobalTicket.TicketManagement.Application.Features.Events.Specifications;
using GlobalTicket.TicketManagement.Domain.Common;
using GlobalTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GlobalTicket.TicketManagement.Application.Features.Events.Queries.GetEventsPage;

public sealed class GetEventsPageQueryHandler : IRequestHandler<GetEventsPageQuery, Page<EventViewModel>>
{
    private readonly IMapper _mapper;
    private readonly IAsyncRepository<Event> _eventRepository;
    private readonly IQuerySpecificationFactory _querySpecificationFactory;

    public GetEventsPageQueryHandler(IMapper mapper, IAsyncRepository<Event> eventRepository, IQuerySpecificationFactory querySpecificationFactory)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        _querySpecificationFactory = querySpecificationFactory ?? throw new ArgumentNullException(nameof(querySpecificationFactory));
    }

    /// <inheritdoc />
    public async Task<Page<EventViewModel>> Handle(GetEventsPageQuery request, CancellationToken cancellationToken)
    {
        IQuerySpecification<Event> query = _querySpecificationFactory.Build<Event>()
            .WithPaging(request.PageRequest)
            .WithOrderBy(new OrderByByDateSpecification());

        return (await _eventRepository.GetPage(query, cancellationToken: cancellationToken))
            .Map<Event, EventViewModel>(_mapper);
    }
}
