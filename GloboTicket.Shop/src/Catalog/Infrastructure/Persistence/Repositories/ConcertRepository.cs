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
        try
        {
            return await _dbContext
                .Concerts
                .FirstOrDefaultAsync(e => e.ConcertId == id, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred");
            throw;
        }
    }

    /// <inheritdoc />
    public async ValueTask<Concert?> GetByQueryAsync(IQuerySpecification<Concert> query, CancellationToken cancellationToken = default)
    {
        return await query
           .ApplySpecifications(_dbContext.Concerts)
           .FirstOrDefaultAsync(cancellationToken)
           .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async ValueTask<TProjection?> GetProjectionByQueryAsync<TProjection>(IQuerySpecification<Concert, TProjection> query,
        CancellationToken cancellationToken = default)
    {
        var queryable = query.ApplySpecifications(_dbContext.Concerts);
        return await query.ApplySelection(queryable).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<Concert> GetAll([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var enumerable = _dbContext.Concerts
            .AsNoTracking()
            .AsAsyncEnumerable();

        await foreach (var concert in enumerable.WithCancellation(cancellationToken))
        {
            yield return concert;
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<TProjection> GetAll<TProjection>(ISelectorSpecification<Concert, TProjection> selector, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var enumerable = selector.ProjectToAsyncEnumerable(_dbContext.Concerts.AsNoTracking(), _queryableToEnumerableConverter);

        await foreach (var projection in enumerable.WithCancellation(cancellationToken))
        {
            yield return projection;
        }
    }

    /// <inheritdoc />
    public async ValueTask<Page<Concert>> GetPage(IQuerySpecification<Concert> query, CancellationToken cancellationToken = default)
    {
        var source = query.ApplySpecifications(_dbContext.Concerts.AsNoTracking());
        var countTask = source.CountAsync(cancellationToken);
        var itemsTask = query.ApplyPaging(source).AsAsyncEnumerable().ToListAsync(cancellationToken).AsTask();
        await Task.WhenAll(countTask, itemsTask).ConfigureAwait(false);

        var count = countTask.Result;
        var items = itemsTask.Result;

        return new Page<Concert>(query.PageNumberOrZero, query.PageSizeOrZero, query.CalculateTotalPages(count), count, items.AsReadOnly());
    }

    /// <inheritdoc />
    public async ValueTask<Page<TProjection>> GetPage<TProjection>(IQuerySpecification<Concert, TProjection> query, CancellationToken cancellationToken = default)
    {
        var source = query.ApplySpecifications(_dbContext.Concerts.AsNoTracking());
        var countTask = source.CountAsync(cancellationToken);
        var itemsTask = query.ApplySelection(query.ApplyPaging(source)).AsAsyncEnumerable().ToListAsync(cancellationToken).AsTask();

        await Task.WhenAll(countTask, itemsTask).ConfigureAwait(false);

        var count = countTask.Result;
        var items = itemsTask.Result;

        return new Page<TProjection>(query.PageNumberOrZero, query.PageSizeOrZero, query.CalculateTotalPages(count), count, items.AsReadOnly());
    }

    /// <inheritdoc />
    public ValueTask<Concert?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Concerts.FindAsync([id], cancellationToken);
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
        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
}
