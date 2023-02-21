namespace GloboTicket.Shop.Shared.Contracts.Persistence.Specifications;

public interface IQuerySpecification<T> where T : class
{
    IQueryable<T> ApplySpecifications(IQueryable<T> source);
    IQueryable<T> ApplyPaging(IQueryable<T> source);

    int PageNumberOrZero { get; }
    int PageSizeOrZero { get; }

    public int CalculateTotalPages(int totalCount)
    {
        return CalculateTotalPages(PageSizeOrZero, totalCount);
    }

    public static int CalculateTotalPages(int pageSize, int totalCount)
    {
        int totalPages = pageSize > 0
            ? (int)Math.Ceiling(totalCount * 1.0 / pageSize)
            : 0;
        return totalPages;
    }
}
public interface IQuerySpecification<T, out TProjection> : IQuerySpecification<T>
    where T : class
{

    IQueryable<TProjection> ApplySelection(IQueryable<T> source);

    IAsyncEnumerable<TProjection> ProjectToAsyncEnumerable(IQueryable<T> source);
}
