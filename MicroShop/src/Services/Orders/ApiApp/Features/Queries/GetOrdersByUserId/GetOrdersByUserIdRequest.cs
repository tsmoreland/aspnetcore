using MediatR;
using MicroShop.Services.Orders.ApiApp.Models;

namespace MicroShop.Services.Orders.ApiApp.Features.Queries.GetOrdersByUserId;

public sealed record class GetOrdersByUserIdRequest(string UserId) : IStreamRequest<OrderHeader>;
