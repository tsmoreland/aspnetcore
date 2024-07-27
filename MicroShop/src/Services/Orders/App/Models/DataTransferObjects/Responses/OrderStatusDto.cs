namespace MicroShop.Services.Orders.App.Models.DataTransferObjects.Responses;

public sealed record class OrderStatusDto(int Id, string? UserId, OrderStatus Status, string? CouponCode, double Discount, double OrderTotal)
{
    public OrderStatusDto(OrderHeader orderHeader)
        : this(orderHeader.Id, orderHeader.UserId, orderHeader.Status, orderHeader.CouponCode, orderHeader.Discount, orderHeader.OrderTotal)
    {
    }
}
