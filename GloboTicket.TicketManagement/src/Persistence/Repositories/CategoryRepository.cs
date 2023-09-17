using GloboTicket.TicketManagement.Domain.Common;
using GloboTicket.TicketManagement.Domain.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Contracts.Persistence.Specifications;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.TicketManagement.Persistence.Repositories;

public sealed class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    /// <inheritdoc />
    public CategoryRepository(GloboTicketDbContext dbContext, IQueryableToEnumerableConverter queryableToEnumerableConverter)
        : base(dbContext, queryableToEnumerableConverter)
    {
    }

    /// <inheritdoc />
    public async ValueTask<Page<Category>> GetPage(int pageNumber, int pageSize, bool includeEvents, CancellationToken cancellationToken)
    {
        IQueryable<Category> categories = DataSet
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
        if (includeEvents)
        {
            categories = categories.Include(e => e.Events);
        }

        Task<int> countTask = DataSet.CountAsync(cancellationToken);
        Task<List<Category>> itemsTask = categories.ToListAsync(cancellationToken);

        await Task.WhenAll(countTask, itemsTask);
        int count = countTask.Result;
        List<Category> items = itemsTask.Result;
        int totalPages = IQuerySpecification<Category>.CalculateTotalPages(pageSize, count);

        return new Page<Category>(pageNumber, pageSize, totalPages, count, items.AsReadOnly());

    }
}
