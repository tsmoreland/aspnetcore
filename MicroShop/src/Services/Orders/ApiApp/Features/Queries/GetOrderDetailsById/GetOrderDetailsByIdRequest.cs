using MediatR;
using MicroShop.Services.Orders.ApiApp.Models;

namespace MicroShop.Services.Orders.ApiApp.Features.Queries.GetOrderDetailsById;

public sealed record class GetOrderDetailsByIdRequest(int OrderId, string? UserId) : IRequest<OrderHeader?>;
