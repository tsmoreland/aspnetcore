using MediatR;
using MicroShop.Services.Orders.App.Models;

namespace MicroShop.Services.Orders.App.Features.Queries.GetOrders;

public sealed record class GetOrdersRequest : IStreamRequest<OrderHeader>;
