using System.Data;
using MediatR;
using MicroShop.Services.Orders.ApiApp.Infrastructure.Data;
using MicroShop.Services.Orders.ApiApp.Models;
using MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Responses;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace MicroShop.Services.Orders.ApiApp.Features.Commands.UpdateOrderStatus;

public sealed class UpdateOrderStatusCommandHandler(AppDbContext dbContext, ILogger<UpdateOrderStatusCommandHandler> logger)
    : IRequestHandler<UpdateOrderStatusCommand, ResponseDto>
{
    /// <inheritdoc />
    public async Task<ResponseDto> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        (int orderId, OrderStatus status) = request;
        try
        {
            OrderHeader? order = await dbContext.OrderHeaders.FindAsync([orderId], cancellationToken);
            if (order == null)
            {
                return ResponseDto.Error("NOTFOUND"); // To Do, replace Rseponse with a result type including error enum value
            }

            if (!order.CanUpdateStatus(status))
            {
                return ResponseDto.Error("BADREQUEST");
            }

            if (status == OrderStatus.Cancelled && order.RefundRequiredOnCancel())
            {
                await IssueRefund(order);
            }

            _ = order.TryUpdateOrderStatus(status);
            // maybe log if the above fails, but it shouldn't because we checked before issuing refund

            await dbContext.SaveChangesAsync(cancellationToken);

            return ResponseDto.Ok();
        }
        catch (DbUpdateException ex)
        {
            // we should perhaps use something like Polly to handle this
            logger.LogError(ex, "Error occured attempting to issue refund");
            return ResponseDto.Error("Failed to change order status, order status may be wrong");
        }
        catch (DBConcurrencyException ex)
        {
            logger.LogError(ex, "Error occured attempting to issue refund");
            return ResponseDto.Error("Failed to change order status, order status may be wrong");
        }
    }

    private async Task<ResponseDto> IssueRefund(OrderHeader order)
    {
        try
        {
            RefundCreateOptions options = new()
            {
                Reason = RefundReasons.RequestedByCustomer,
                PaymentIntent = order.PaymentIntentId,

            };
            RefundService service = new();
            Refund refund = await service.CreateAsync(options);

            return ResponseDto.Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured attempting to issue refund");
            return ResponseDto.Error(ex.Message);
        }
    }
}
