using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Contracts.Persistence.Specifications;
using GloboTicket.TicketManagement.Application.Features.Orders.Specifications;
using GloboTicket.TicketManagement.Domain.Common;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Orders.Queries.GetOrdersForMonth;

public sealed class GetOrdersForMonthQueryHandler : IRequestHandler<GetOrdersForMonthQuery, Page<OrderViewModel>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IQueryBuilderFactory _queryBuilderFactory;

    public GetOrdersForMonthQueryHandler(IOrderRepository orderRepository, IQueryBuilderFactory queryBuilderFactory)
    {
        _orderRepository = orderRepository;
        _queryBuilderFactory = queryBuilderFactory;
    }

    /// <inheritdoc />
    public async Task<Page<OrderViewModel>> Handle(GetOrdersForMonthQuery request, CancellationToken cancellationToken)
    {
        IQuerySpecification<Order, OrderViewModel> query = _queryBuilderFactory.Build<Order>()
            .WithNoTracking()
            .WithFilter(new ForMonthFilterSpecification(request.Date))
            .WithOrderBy(new OrderByOrderPlacedSpecification())
            .WithPaging(new PageRequest(request.PageNumber, request.PageSize))
            .Query(new OrderViewModelSelectionSpecification());

        Page<OrderViewModel> page = await _orderRepository.GetPage(query, cancellationToken);
        return page;
    }
}
