using GlobalTicket.TicketManagement.Domain.Common;

namespace GlobalTicket.TicketManagement.Application.Contracts.Persistence;

public interface IAsyncRepository<T> where T : class
{
    ValueTask<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    ValueTask<Page<T>> GetPage(
        PageRequest pageRequest,
        IFilterSpecification<T>? filter = null,
        CancellationToken cancellationToken = default);

    ValueTask<Page<T>> GetPage<TKey>(
        PageRequest pageRequest,
        IFilterSpecification<T>? filter = null,
        IOrderBySpecification<T, TKey>? orderBy = null,
        CancellationToken cancellationToken = default);

    ValueTask<Page<TProjection>> GetPage<TProjection, TKey>(
        PageRequest pageRequest,
        ISelector<T, TProjection>? selector,
        IFilterSpecification<T>? filter = null,
        IOrderBySpecification<T, TKey>? orderBy = null,
        CancellationToken cancellationToken = default);

    ValueTask<T> AddAsync(T entity);

    ValueTask<T> UpdateAsync(T entity);

    ValueTask DeleteAsync(T entity);
}
