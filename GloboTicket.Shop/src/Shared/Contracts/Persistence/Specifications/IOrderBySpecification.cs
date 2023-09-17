
namespace GloboTicket.Shop.Shared.Contracts.Persistence.Specifications;

public interface IOrderBySpecification<T> where T : class
{
    IQueryable<T> ApplyOrderBy(IQueryable<T> source);
}
