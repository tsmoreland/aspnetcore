using GlobalTicket.TicketManagement.Domain.Entities;

namespace GlobalTicket.TicketManagement.Application.Contracts.Persistence;

public interface ICategoryRepository : IAsyncRepository<Event>
{
}