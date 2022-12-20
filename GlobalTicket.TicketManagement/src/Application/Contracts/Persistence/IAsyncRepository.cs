using GlobalTicket.TicketManagement.Domain.Common;

namespace GlobalTicket.TicketManagement.Application.Contracts.Persistence;

public interface IAsyncRepository<T> where T : class
{
    ValueTask<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    ValueTask<Page<T>> GetPage(PageRequest pageRequest, CancellationToken cancellationToken);

    ValueTask<Page<T>> GetFiltered(PageRequest pageRequest, IFilterSpecification<T> filter, CancellationToken cancellationToken);

    ValueTask<Page<TProjection>>  GetFilteredProjection<TProjection>(
        PageRequest pageRequest, 
        IFilterSpecification<T> filter,
        ISelector<T, TProjection> selector,
        CancellationToken cancellationToken);

    ValueTask<T> AddAsync(T entity);

    ValueTask<T> UpdateAsync(T entity);

    ValueTask DeleteAsync(T entity);
}
