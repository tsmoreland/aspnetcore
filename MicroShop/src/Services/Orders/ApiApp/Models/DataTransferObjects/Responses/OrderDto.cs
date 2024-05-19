namespace MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Responses;

public sealed record class OrderDto(int Id, string UserId, OrderStatus Status, string Name, string EmailAddress,
    double OrderTotal, IEnumerable<OrderItemDto> Items)
{
    public OrderDto(OrderHeader header)
        : this(header.Id, header.UserId ?? "Anonymous", header.Status, header.Name, header.EmailAddress,  header.OrderTotal, header.Items.Select(i => new OrderItemDto(i)).ToList())
    {
    }
}
