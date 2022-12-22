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

        Task<int> countTask =  DataSet.CountAsync(cancellationToken);
        Task<List<Category>> itemsTask = categories.ToListAsync(cancellationToken);

        await Task.WhenAll(countTask, itemsTask);
        int count = countTask.Result;
        List<Category> items = itemsTask.Result;
        int totalPages = IQuerySpecification<Category>.CalculateTotalPages(pageSize, count);

        return new Page<Category>(pageNumber, pageSize, totalPages, count, items.AsReadOnly());

    }
}
