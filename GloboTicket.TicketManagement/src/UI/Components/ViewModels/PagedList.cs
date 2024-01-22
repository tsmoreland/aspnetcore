namespace GloboTicket.TicketManagement.UI.Components.ViewModels;

public sealed class PagedList<T>
{
    private readonly List<T> _items = [];
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }

    public IEnumerable<T> Items => _items.AsReadOnly().AsEnumerable();

    public PagedList()
    {
    }

    public PagedList(IEnumerable<T> items, int pageIndex, int totalPages)
    {
        _items.AddRange(items);
        PageIndex = pageIndex;
        TotalPages = totalPages;
    }

    public bool HasPreviousPage => (PageIndex > 1);

    public bool HasNextPage => (PageIndex < TotalPages);
}
