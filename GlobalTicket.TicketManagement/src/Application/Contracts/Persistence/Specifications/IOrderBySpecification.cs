
using System.Linq.Expressions;

namespace GlobalTicket.TicketManagement.Application.Contracts.Persistence.Specifications;

public interface IOrderBySpecification<T> where T : class
{
    IQueryable<T> ApplyOrder(IQueryable<T> source);
}

public interface IOrderBySpecification<T, TKey> : IOrderBySpecification<T>
    where T : class
{
    Expression<Func<T, TKey>> OrderBy { get; }
}
