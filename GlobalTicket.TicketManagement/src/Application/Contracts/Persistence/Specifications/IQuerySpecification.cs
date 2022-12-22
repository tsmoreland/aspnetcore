using System.Linq.Expressions;
using GlobalTicket.TicketManagement.Domain.Common;

namespace GlobalTicket.TicketManagement.Application.Contracts.Persistence.Specifications;

public interface IQuerySpecification<T> where T : class
{
    bool DoNotTrack { get; }
    IEnumerable<string> Inclusions { get; }
    Expression<Func<T, bool>>? Filter { get; }
    PageRequest? PageRequest { get; }
    Expression<Func<T, object?>>? OrderBy { get; }
}
public interface IQuerySpecification<T, TProjection> : IQuerySpecification<T>
    where T : class
{
    Expression<Func<T, TProjection>> Selector { get; }

}
