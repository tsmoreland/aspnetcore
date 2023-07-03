using GloboTicket.TicketManagement.Domain.Common;
using GloboTicket.TicketManagement.Domain.Contracts.Persistence.Specifications;

namespace GloboTicket.TicketManagement.Domain.Contracts.Persistence;

public interface IAsyncRepository<T> where T : class
{
    ValueTask<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    ValueTask<T?> GetByQueryAsync(IQuerySpecification<T> query, CancellationToken cancellationToken = default);
    ValueTask<TProjection?> GetProjectionByQueryAsync<TProjection>(IQuerySpecification<T, TProjection> query, CancellationToken cancellationToken = default);

    IAsyncEnumerable<T> GetAll(CancellationToken cancellationToken = default);
    IAsyncEnumerable<TProjection> GetAll<TProjection>(ISelectorSpecification<T, TProjection> selector, CancellationToken cancellationToken = default);

    ValueTask<Page<T>> GetPage(
        IQuerySpecification<T> query,
        CancellationToken cancellationToken = default);

    ValueTask<Page<TProjection>> GetPage<TProjection>(
        IQuerySpecification<T, TProjection> query,
        CancellationToken cancellationToken = default);

    ValueTask<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    ValueTask UpdateAsync(T entity, CancellationToken cancellationToken = default);

    ValueTask DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
