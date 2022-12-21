using GlobalTicket.TicketManagement.Domain.Common;
using MediatR;

namespace GlobalTicket.TicketManagement.Application.Features.Events.Queries.GetEventsPage;

public sealed record class GetEventsPageQuery(PageRequest PageRequest) : IRequest<Page<EventViewModel>>;
