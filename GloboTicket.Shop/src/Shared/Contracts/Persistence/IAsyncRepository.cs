namespace GloboTicket.Shop.Shared.Contracts.Persistence;

public interface IAsyncRepository<T> : IAsyncReadOnlyRepository<T>
    where T : class
{
    ValueTask<T?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);

    ValueTask SaveChangesAsync(CancellationToken cancellationToken);
    void SaveChanges();
}
