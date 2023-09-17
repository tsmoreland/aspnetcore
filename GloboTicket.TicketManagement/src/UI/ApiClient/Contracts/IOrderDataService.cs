using GloboTicket.TicketManagement.UI.ApiClient.ViewModels;

namespace GloboTicket.TicketManagement.UI.ApiClient.Contracts;

public interface IOrderDataService
{
    ValueTask<Page<OrdersForMonthListViewModel>> GetPagedOrderForMonth(int pageNumber, int pageSize, DateTime date, CancellationToken cancellationToken);   
}
