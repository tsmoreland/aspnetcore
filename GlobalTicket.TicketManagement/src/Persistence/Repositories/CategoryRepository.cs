using GlobalTicket.TicketManagement.Application.Contracts.Persistence;
using GlobalTicket.TicketManagement.Application.Contracts.Persistence.Specifications;
using GlobalTicket.TicketManagement.Domain.Common;
using GlobalTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GlobalTicket.TicketManagement.Persistence.Repositories;

public sealed class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    /// <inheritdoc />
    public CategoryRepository(GlobalTicketDbContext dbContext) : base(dbContext)
    {
    }

    /// <inheritdoc />
    public ValueTask<Page<Category>> GetPage(IQuerySpecification<Category> query, bool includeEvents, CancellationToken cancellationToken)
    {
        IQueryable<Category> source = ApplyQuerySpecification(DataSet, query);

        if (includeEvents)
        {
            source = source.Include(e => e.Events);
        }

        return GetPageAsync(source, query.PageRequest!, cancellationToken);
    }
}
