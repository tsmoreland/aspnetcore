using GlobalTicket.TicketManagement.Domain.Entities;

namespace GlobalTicket.TicketManagement.Application.Contracts.Persistence;

public interface IEventRepository : IAsyncRepository<Event>
{
}
