//
// Copyright © 2023 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Runtime.CompilerServices;
using GloboTicket.Shop.Catalog.Domain.Contracts.Persistence;
using GloboTicket.Shop.Catalog.Domain.Models;
using GloboTicket.Shop.Shared.Contracts.Persistence;
using GloboTicket.Shop.Shared.Contracts.Persistence.Specifications;
using GloboTicket.Shop.Shared.Models.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GloboTicket.Shop.Catalog.Infrastructure.Persistence.Repositories;

public sealed class ConcertRepository : IConcertRepository, IReadOnlyConcertRepository
{
    private readonly EventCatalogDbContext _dbContext;
    private readonly IQueryableToEnumerableConverter _queryableToEnumerableConverter;
    private readonly ILogger<ConcertRepository> _logger;

    public ConcertRepository(
        EventCatalogDbContext dbContext,
        IQueryableToEnumerableConverter queryableToEnumerableConverter,
        ILoggerFactory loggerFactory)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _queryableToEnumerableConverter = queryableToEnumerableConverter ?? throw new ArgumentNullException(nameof(queryableToEnumerableConverter));

        ArgumentNullException.ThrowIfNull(loggerFactory);
        _logger = loggerFactory.CreateLogger<ConcertRepository>();
    }

    /// <inheritdoc />
    public async ValueTask<Concert?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Concerts
            .FirstOrDefaultAsync(e => e.ConcertId == id, cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask<Concert?> GetByQueryAsync(IQuerySpecification<Concert> query, CancellationToken cancellationToken = default)
    {
        return await query
           .ApplySpecifications(_dbContext.Concerts)
           .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask<TProjection?> GetProjectionByQueryAsync<TProjection>(IQuerySpecification<Concert, TProjection> query,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Concert> queryable = query.ApplySpecifications(_dbContext.Concerts);
        return await query.ApplySelection(queryable).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<Concert> GetAll([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        IAsyncEnumerable<Concert> enumerable = _dbContext.Concerts
            .AsNoTracking()
            .AsAsyncEnumerable();

        await foreach (Concert concert in enumerable.WithCancellation(cancellationToken))
        {
            yield return concert;
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<TProjection> GetAll<TProjection>(ISelectorSpecification<Concert, TProjection> selector, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        IAsyncEnumerable<TProjection> enumerable = selector.ProjectToAsyncEnumerable(_dbContext.Concerts.AsNoTracking(), _queryableToEnumerableConverter);

        await foreach (TProjection projection in enumerable.WithCancellation(cancellationToken))
        {
            yield return projection;
        }
    }

    /// <inheritdoc />
    public async ValueTask<Page<Concert>> GetPage(IQuerySpecification<Concert> query, CancellationToken cancellationToken = default)
    {
        IQueryable<Concert> source = query.ApplySpecifications(_dbContext.Concerts.AsNoTracking());
        Task<int> countTask = source.CountAsync(cancellationToken);
        Task<List<Concert>> itemsTask = query.ApplyPaging(source).AsAsyncEnumerable().ToListAsync(cancellationToken).AsTask();
        await Task.WhenAll(countTask, itemsTask);

        int count = countTask.Result;
        List<Concert> items = itemsTask.Result;

        return new Page<Concert>(query.PageNumberOrZero, query.PageSizeOrZero, query.CalculateTotalPages(count), count, items.AsReadOnly());
    }

    /// <inheritdoc />
    public async ValueTask<Page<TProjection>> GetPage<TProjection>(IQuerySpecification<Concert, TProjection> query, CancellationToken cancellationToken = default)
    {
        IQueryable<Concert> source = query.ApplySpecifications(_dbContext.Concerts.AsNoTracking());
        Task<int> countTask = source.CountAsync(cancellationToken);
        Task<List<TProjection>> itemsTask = query.ApplySelection(query.ApplyPaging(source)).AsAsyncEnumerable().ToListAsync(cancellationToken).AsTask();

        await Task.WhenAll(countTask, itemsTask);

        int count = countTask.Result;
        List<TProjection> items = itemsTask.Result;

        return new Page<TProjection>(query.PageNumberOrZero, query.PageSizeOrZero, query.CalculateTotalPages(count), count, items.AsReadOnly());
    }

    /// <inheritdoc />
    public ValueTask<Concert?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Concerts.FindAsync(new object[] { id }, cancellationToken);
    }

    /// <inheritdoc />
    public void Add(Concert entity)
    {
        _dbContext.Concerts.Add(entity);
    }

    /// <inheritdoc />
    public void Update(Concert entity)
    {
        _dbContext.Concerts.Update(entity);
    }

    /// <inheritdoc />
    public void Delete(Concert entity)
    {
        _dbContext.Concerts.Remove(entity);
    }

    /// <inheritdoc />
    public async ValueTask SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
}
