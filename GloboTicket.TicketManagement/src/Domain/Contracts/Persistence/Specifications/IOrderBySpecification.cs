
namespace GloboTicket.TicketManagement.Domain.Contracts.Persistence.Specifications;

public interface IOrderBySpecification<T> where T : class
{
    IQueryable<T> ApplyOrderBy(IQueryable<T> source);
}
