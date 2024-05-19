namespace MicroShop.Web.MvcApp.Models.Orders;

public sealed record class OrderDto(int Id, string UserId, OrderStatus Status, string Name, string Email, double OrderTotal, IEnumerable<OrderItemDto> Items);
