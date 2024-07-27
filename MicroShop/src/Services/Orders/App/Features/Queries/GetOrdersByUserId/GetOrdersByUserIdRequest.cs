using MediatR;
using MicroShop.Services.Orders.App.Models;

namespace MicroShop.Services.Orders.App.Features.Queries.GetOrdersByUserId;

public sealed record class GetOrdersByUserIdRequest(string UserId) : IStreamRequest<OrderHeader>;
