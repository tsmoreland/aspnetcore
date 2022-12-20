using MediatR;

namespace GlobalTicket.TicketManagement.Application.Features.Events;

public sealed record class GetEventDetailQuery(Guid Id) : IRequest<EventDetailViewModel?>;
