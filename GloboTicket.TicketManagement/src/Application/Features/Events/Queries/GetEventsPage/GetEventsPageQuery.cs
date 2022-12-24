using GloboTicket.TicketManagement.Domain.Common;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsPage;

public sealed record class GetEventsPageQuery(PageRequest PageRequest) : IRequest<Page<EventViewModel>>;
