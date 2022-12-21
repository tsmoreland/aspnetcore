
using System.Linq.Expressions;

namespace GlobalTicket.TicketManagement.Application.Contracts.Persistence.Specifications;

public interface IOrderBySpecification<T, TKey> where T : class
{
    Expression<Func<T, TKey>> OrderBy { get; }
}
