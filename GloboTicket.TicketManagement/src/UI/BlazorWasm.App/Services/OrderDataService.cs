using AutoMapper;
using GloboTicket.TicketManagement.UI.ApiClient.Contracts;
using GloboTicket.TicketManagement.UI.ApiClient.Services;
using GloboTicket.TicketManagement.UI.ApiClient.ViewModels;

namespace GloboTicket.TicketManagement.UI.BlazorWasm.App.Services;

public sealed class OrderDataService : BaseDataService, IOrderDataService
{
    /// <inheritdoc />
    public OrderDataService(IClient client, IMapper mapper, ITokenRepository tokenRepository)
        : base(client, mapper, tokenRepository)
    {
    }

    /// <inheritdoc />
    public ValueTask<Page<OrdersForMonthListViewModel>> GetPagedOrderForMonth(int pageNumber, int pageSize, DateTime date, CancellationToken cancellationToken)
    {
        return ValueTask.FromException<Page<OrdersForMonthListViewModel>>(new NotImplementedException());
    }
}
