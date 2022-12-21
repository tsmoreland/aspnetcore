using GlobalTicket.TicketManagement.Application.Contracts.Persistence.Specifications;
using GlobalTicket.TicketManagement.Domain.Common;
using GlobalTicket.TicketManagement.Domain.Entities;

namespace GlobalTicket.TicketManagement.Application.Contracts.Persistence;

public interface ICategoryRepository : IAsyncRepository<Category>
{
    ValueTask<Page<Category>> GetPage(IQuerySpecification<Category> query, bool includeEvents, CancellationToken cancellationToken);
}
