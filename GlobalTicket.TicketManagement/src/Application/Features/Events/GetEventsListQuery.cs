using GlobalTicket.TicketManagement.Domain.Common;
using MediatR;

namespace GlobalTicket.TicketManagement.Application.Features.Events;

public sealed record class GetEventsListQuery(PageRequest PageRequest) : IRequest<Page<EventViewModel>>;
