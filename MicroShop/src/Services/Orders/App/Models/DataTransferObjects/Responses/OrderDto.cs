namespace MicroShop.Services.Orders.App.Models.DataTransferObjects.Responses;

public sealed record class OrderDto(int Id, string UserId, OrderStatus Status, string Name, string EmailAddress,
    double OrderTotal, DateTime OrderTime, string? StripeSessionId, string? PaymentIntentId,
    IEnumerable<OrderItemDto> Items)
{
    public OrderDto(OrderHeader header)
        : this(header.Id, header.UserId ?? "Anonymous", header.Status, header.Name, header.EmailAddress,
            header.OrderTotal,
            header.OrderTime,
            header.StripeSessionId,
            header.PaymentIntentId,
            header.Items.Select(i => new OrderItemDto(i)).ToList())
    {
    }
}
