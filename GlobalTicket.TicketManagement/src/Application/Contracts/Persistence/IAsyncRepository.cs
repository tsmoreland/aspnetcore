using GlobalTicket.TicketManagement.Application.Contracts.Persistence.Specifications;
using GlobalTicket.TicketManagement.Domain.Common;

namespace GlobalTicket.TicketManagement.Application.Contracts.Persistence;

public interface IAsyncRepository<T> where T : class
{
    ValueTask<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    ValueTask<T?> GetProjectionByIdAsync<TProjection>(Guid id, IQuerySpecification<T, TProjection> query, CancellationToken cancellationToken = default);

    ValueTask<Page<T>> GetPage(
        IQuerySpecification<T> query,
        CancellationToken cancellationToken = default);

    ValueTask<Page<TProjection>> GetPage<TProjection>(
        IQuerySpecification<T, TProjection> query,
        CancellationToken cancellationToken = default);

    ValueTask<T> AddAsync(T entity);

    ValueTask<T> UpdateAsync(T entity);

    ValueTask DeleteAsync(T entity);
}
