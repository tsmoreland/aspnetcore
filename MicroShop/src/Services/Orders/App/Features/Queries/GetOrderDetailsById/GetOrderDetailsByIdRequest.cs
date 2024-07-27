using MediatR;
using MicroShop.Services.Orders.App.Models;

namespace MicroShop.Services.Orders.App.Features.Queries.GetOrderDetailsById;

public sealed record class GetOrderDetailsByIdRequest(int OrderId, string? UserId) : IRequest<OrderHeader?>;
