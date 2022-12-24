using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;

public sealed record class GetEventDetailQuery(Guid Id) : IRequest<EventDetailViewModel?>;
