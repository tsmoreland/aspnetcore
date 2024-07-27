using MediatR;
using MicroShop.Services.Orders.App.Models;
using MicroShop.Services.Orders.App.Models.DataTransferObjects.Responses;

namespace MicroShop.Services.Orders.App.Features.Commands.UpdateOrderStatus;

public sealed record class UpdateOrderStatusCommand(int OrderId, OrderStatus Status) : IRequest<ResponseDto>;
