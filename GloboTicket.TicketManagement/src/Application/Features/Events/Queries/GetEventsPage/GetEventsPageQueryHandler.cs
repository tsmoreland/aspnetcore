﻿using AutoMapper;
using GloboTicket.TicketManagement.Application.Features.Events.Specifications;
using GloboTicket.TicketManagement.Domain.Common;
using GloboTicket.TicketManagement.Domain.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Contracts.Persistence.Specifications;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsPage;

public sealed class GetEventsPageQueryHandler : IRequestHandler<GetEventsPageQuery, Page<EventViewModel>>
{
    private readonly IMapper _mapper;
    private readonly IAsyncRepository<Event> _eventRepository;
    private readonly IQueryBuilderFactory _querySpecificationFactory;

    public GetEventsPageQueryHandler(IMapper mapper, IAsyncRepository<Event> eventRepository, IQueryBuilderFactory querySpecificationFactory)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        _querySpecificationFactory = querySpecificationFactory ?? throw new ArgumentNullException(nameof(querySpecificationFactory));
    }

    /// <inheritdoc />
    public async Task<Page<EventViewModel>> Handle(GetEventsPageQuery request, CancellationToken cancellationToken)
    {
        IQueryBuilder<Event> queryBuilder = _querySpecificationFactory.Build<Event>()
            .WithPaging(request.PageRequest)
            .WithOrderBy(new OrderByByDateSpecification());

        return (await _eventRepository.GetPage(queryBuilder.Query(), cancellationToken: cancellationToken))
            .Map<Event, EventViewModel>(_mapper);
    }
}
