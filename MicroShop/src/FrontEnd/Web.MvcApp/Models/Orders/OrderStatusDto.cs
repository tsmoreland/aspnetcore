namespace MicroShop.Web.MvcApp.Models.Orders;

public sealed record class OrderStatusDto(int Id, string? UserId, OrderStatus Status, string? CouponCode, double Discount, double OrderTotal);
