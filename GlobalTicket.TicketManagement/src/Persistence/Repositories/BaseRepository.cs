// 
// Copyright © 2022 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using GlobalTicket.TicketManagement.Application.Contracts.Persistence;
using GlobalTicket.TicketManagement.Application.Contracts.Persistence.Specifications;
using GlobalTicket.TicketManagement.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace GlobalTicket.TicketManagement.Persistence.Repositories;

public class BaseRepository<T> : IAsyncRepository<T> where T : class
{
    private readonly GlobalTicketDbContext _dbContext;

    protected DbSet<T> DataSet => _dbContext.Set<T>();

    public BaseRepository(GlobalTicketDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <inheritdoc />
    public ValueTask<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);
    }

    /// <inheritdoc />
    public virtual async ValueTask<T?> GetByQueryAsync(IQuerySpecification<T> query, CancellationToken cancellationToken = default)
    {
        return await query.ApplySpecifications(_dbContext.Set<T>()).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public virtual async ValueTask<TProjection?> GetProjectionByQueryAsync<TProjection>(IQuerySpecification<T, TProjection> query, CancellationToken cancellationToken = default)
    {
        return await query.ApplySelection(query.ApplySpecifications(_dbContext.Set<T>())).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask<Page<T>> GetPage(IQuerySpecification<T> query, CancellationToken cancellationToken = default)
    {
        // todo: validate the query
        IQueryable<T> source = query.ApplySpecifications(DataSet);
        Task<int> countTask = source.CountAsync(cancellationToken);
        Task<List<T>> itemsTask = query.ApplyPaging(source).AsAsyncEnumerable().ToListAsync(cancellationToken).AsTask();

        await Task.WhenAll(countTask, itemsTask);

        int count = countTask.Result;
        List<T> items = itemsTask.Result;

        return new Page<T>(query.PageNumberOrZero, query.PageSizeOrZero, query.CalculateTotalPages(count), count, items.AsReadOnly());
    }

    /// <inheritdoc />
    public async ValueTask<Page<TProjection>> GetPage<TProjection>(IQuerySpecification<T, TProjection> query, CancellationToken cancellationToken = default)
    {
        IQueryable<T> source = query.ApplySpecifications(DataSet);
        Task<int> countTask = source.CountAsync(cancellationToken);
        Task<List<TProjection>> itemsTask = query.ApplySelection(query.ApplyPaging(source)).AsAsyncEnumerable().ToListAsync(cancellationToken).AsTask();

        await Task.WhenAll(countTask, itemsTask);

        int count = countTask.Result;
        List<TProjection> items = itemsTask.Result;

        return new Page<TProjection>(query.PageNumberOrZero, query.PageSizeOrZero, query.CalculateTotalPages(count), count, items.AsReadOnly());
    }

    /// <inheritdoc />
    public async ValueTask<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    /// <inheritdoc />
    public async ValueTask UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    protected async ValueTask<Page<T>> GetPage(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        Task<int> countTask = source.CountAsync(cancellationToken);
        Task<List<T>> itemsTask = source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        await Task.WhenAll(countTask, itemsTask);
        int count = countTask.Result;
        List<T> items = itemsTask.Result;

        int totalPages = IQuerySpecification<T>.CalculateTotalPages(pageSize, count);

        return new Page<T>(pageNumber, pageSize, totalPages, count, items.AsReadOnly());
    }

}
