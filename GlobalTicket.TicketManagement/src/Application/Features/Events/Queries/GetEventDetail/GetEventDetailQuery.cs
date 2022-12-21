using MediatR;

namespace GlobalTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;

public sealed record class GetEventDetailQuery(Guid Id) : IRequest<EventDetailViewModel?>;
