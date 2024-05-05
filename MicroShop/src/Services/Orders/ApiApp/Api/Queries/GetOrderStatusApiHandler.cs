using MicroShop.Services.Orders.ApiApp.Infrastructure.Data;
using MicroShop.Services.Orders.ApiApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace MicroShop.Services.Orders.ApiApp.Api.Queries;

public sealed class GetOrderStatusApiHandler(AppDbContext dbContext, ILogger<GetOrderStatusApiHandler> logger)
{
    private const string PaymentSuccess = "succeeded";

    public async Task<OrderHeader?> Handle([FromRoute] int orderId)
    {
        try
        {
            OrderHeader? order = await dbContext.OrderHeaders
                .Where(e => e.Id == orderId)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
            if (order is null)
            {
                return null;
            }

            if (order.PaymentIntentId is { Length: > 0 })
            {
                return order;
            }

            SessionService sessionService = new();
            Session session = await sessionService.GetAsync(order.StripeSessionId);

            PaymentIntentService paymentIntentService = new();
            PaymentIntent payment = await paymentIntentService.GetAsync(session.PaymentIntentId);

            if (payment.Status != PaymentSuccess)
            {
                return order;
            }

            order.SetPaymentIntentIdIfPending(payment.Id);
            await dbContext.SaveChangesAsync();

            return order;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred.");
            return null;
        }
    }
}
