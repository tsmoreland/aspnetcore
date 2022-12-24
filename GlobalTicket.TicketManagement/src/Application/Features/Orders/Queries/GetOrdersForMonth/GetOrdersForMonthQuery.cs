using GloboTicket.TicketManagement.Domain.Common;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Orders.Queries.GetOrdersForMonth;

public sealed record class GetOrdersForMonthQuery(DateTime Date, int PageNumber, int PageSize) : IRequest<Page<OrderViewModel>>;

