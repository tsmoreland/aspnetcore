namespace MicroShop.Services.Orders.App.Models.DataTransferObjects.Responses;

public sealed record OrderSummaryDto(
    int Id,
    OrderStatus Status,
    string? CouponCode,
    double Discount,
    double OrderTotal,
    IEnumerable<OrderItemDto> Details)
{
    public OrderSummaryDto(OrderHeader order)
        : this(order.Id, order.Status, order.CouponCode, order.Discount, order.OrderTotal, order.Items.Select(li => new OrderItemDto(li)))
    {
    }
}
