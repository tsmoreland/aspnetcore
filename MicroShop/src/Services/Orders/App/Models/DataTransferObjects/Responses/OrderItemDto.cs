namespace MicroShop.Services.Orders.App.Models.DataTransferObjects.Responses;

public sealed record class OrderItemDto(
    int Id,
    int ProductId,
    string ProductName,
    double Price,
    int Count)
{
    public OrderItemDto(OrderDetails details)
        : this(details.Id, details.ProductId, details.ProductName, details.Price, details.Count)
    {
    }
}
