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
        return await ApplyQuerySpecification(_dbContext.Set<T>(), query).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public virtual async ValueTask<TProjection?> GetProjectionByQueryAsync<TProjection>(IQuerySpecification<T, TProjection> query, CancellationToken cancellationToken = default)
    {
        return await ApplyQuerySpecification(_dbContext.Set<T>(), query).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public ValueTask<Page<T>> GetPage(IQuerySpecification<T> query, CancellationToken cancellationToken = default)
    {
        // todo: validate the query
        return GetPageAsync(ApplyQuerySpecification(_dbContext.Set<T>(), query), query.PageRequest!, cancellationToken);
    }

    /// <inheritdoc />
    public ValueTask<Page<TProjection>> GetPage<TProjection>(IQuerySpecification<T, TProjection> query, CancellationToken cancellationToken = default)
    {
        return GetPageAsync(ApplyQuerySpecification(_dbContext.Set<T>(), query), query.PageRequest!, cancellationToken);
    }
    protected virtual async ValueTask<Page<TValue>> GetPageAsync<TValue>(IQueryable<TValue> source, PageRequest pageRequest, CancellationToken cancellationToken)
    {
        Task<int> countTask = source.CountAsync(cancellationToken);
        Task<List<TValue>> itemsTask = ApplyPaging(source, pageRequest).AsAsyncEnumerable().ToListAsync(cancellationToken).AsTask();

        await Task.WhenAll(countTask, itemsTask);

        int count = countTask.Result;
        List<TValue> items = itemsTask.Result;

        (int pageNumber, int pageSize, int totalPages) = GetPageDetails(pageRequest, count);
        return new Page<TValue>(pageNumber, pageSize, totalPages, count, items.AsReadOnly());
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

    protected virtual (int PageNumber, int PageSize, int TotalPages) GetPageDetails(PageRequest pageRequest, int totalCount)
    {
        int totalPages = (int)Math.Ceiling(totalCount * 1.0 / pageRequest.PageSize);
        return (pageRequest.PageNumber, pageRequest.PageSize, totalPages);
    }

    protected virtual IQueryable<T> ApplyQuerySpecification(IQueryable<T> source, IQuerySpecification<T> query)
    {
        if (query.DoNotTrack)
        {
            source = source.AsNoTracking();
        }

        if (query.Filter is not null)
        {
            source = source.Where(query.Filter);
        }

        if (query.OrderBy is not null)
        {
            source = source.OrderBy(query.OrderBy);
        }

        return source;
    }

    protected virtual IQueryable<T> ApplyQuerySpecificationWithPaging(IQueryable<T> source, IQuerySpecification<T> query)
    {
        source = ApplyQuerySpecification(source, query);
        if (query.PageRequest is not null)
        {
            source = source
                .Skip((query.PageRequest.PageNumber - 1) * query.PageRequest.PageSize)
                .Take(query.PageRequest.PageSize);
        }
        return source;
    }
    protected virtual IQueryable<T> ApplyPaging(IQueryable<T> source, IQuerySpecification<T> query)
    {
        return ApplyPaging(source, query.PageRequest);
    }
    protected virtual IQueryable<TValue> ApplyPaging<TValue>(IQueryable<TValue> source, PageRequest? pageRequest)
    {
        if (pageRequest is null)
        {
            return source;
        }

        return source
            .Skip((pageRequest.PageNumber - 1) * pageRequest.PageSize)
            .Take(pageRequest.PageSize);
    }
    protected virtual IQueryable<TProjection> ApplyQuerySpecification<TProjection>(IQueryable<T> source, IQuerySpecification<T, TProjection> query)
    {
        source = ApplyQuerySpecificationWithPaging(source, query);
        return source.Select(query.Selector);
    }


}
