using MediatR;
using MicroShop.Services.Orders.ApiApp.Models;

namespace MicroShop.Services.Orders.ApiApp.Features.Queries.GetOrders;

public sealed record class GetOrdersRequest : IStreamRequest<OrderHeader>;
