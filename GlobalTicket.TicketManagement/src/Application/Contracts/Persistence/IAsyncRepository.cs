using GlobalTicket.TicketManagement.Application.Contracts.Persistence.Specifications;
using GlobalTicket.TicketManagement.Domain.Common;

namespace GlobalTicket.TicketManagement.Application.Contracts.Persistence;

public interface IAsyncRepository<T> where T : class
{
    ValueTask<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    ValueTask<T?> GetByQueryAsync(IQuerySpecification<T> query, CancellationToken cancellationToken = default);
    ValueTask<TProjection?> GetProjectionByQueryAsync<TProjection>(IQuerySpecification<T, TProjection> query, CancellationToken cancellationToken = default);

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
