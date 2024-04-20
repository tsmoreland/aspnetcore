using MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Responses;

namespace MicroShop.Services.Orders.ApiApp.Models;

public sealed class OrderDetails
{
    public OrderDetails(OrderHeader header, int productId, string productName, double price, int count)
        : this(0, header.Id, productId, productName, price, count)
    {
        Header = header;
    }


    private OrderDetails(int id, int headerId, int productId, string productName, double price, int count)
    {
        Id = id;
        HeaderId = headerId;
        ProductId = productId;
        ProductName = productName;
        Price = price;
        Count = count;
    }


    public int Id { get; private init; }
    public int HeaderId { get; private init; }
    public OrderHeader? Header { get; private init; }
    public int ProductId { get; private init; }
    public string ProductName { get; private init; }
    public double Price { get; private init; }
    public int Count { get; private init; }

}
