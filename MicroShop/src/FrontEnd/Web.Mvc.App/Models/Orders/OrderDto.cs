namespace MicroShop.Web.Mvc.App.Models.Orders;

public sealed record class OrderDto(int Id, string UserId, OrderStatus Status, string Name, string Email,
    double OrderTotal, DateTime OrderTime, string? StripeSessionId, string? PaymentIntentId,
    IEnumerable<OrderItemDto> Items);
