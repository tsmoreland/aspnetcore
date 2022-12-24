using GlobalTicket.TicketManagement.Domain.Common;
using MediatR;

namespace GlobalTicket.TicketManagement.Application.Features.Orders.Queries.GetOrdersForMonth;

public sealed record class GetOrdersForMonthQuery(DateTime Date, int PageNumber, int PageSize) : IRequest<Page<OrderViewModel>>;

