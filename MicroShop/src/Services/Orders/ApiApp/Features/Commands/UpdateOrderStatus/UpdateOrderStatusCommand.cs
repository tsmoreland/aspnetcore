using MediatR;
using MicroShop.Services.Orders.ApiApp.Models;
using MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Responses;

namespace MicroShop.Services.Orders.ApiApp.Features.Commands.UpdateOrderStatus;

public sealed record class UpdateOrderStatusCommand(int OrderId, OrderStatus Status) : IRequest<ResponseDto>;
