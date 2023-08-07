namespace BethanysPieShop.Admin.Domain.Models;

public sealed class Order
{
    private readonly HashSet<OrderDetail> _orderDetails = new();

    public Guid Id { get; }
    public ICollection<OrderDetail> OrderDetails => _orderDetails.ToList();
    public OrderStatus OrderStatus { get; set; }
    public string FirstName { get; set; } = string.Empty; // TODO add validators
    public string LastName { get; set; } = string.Empty; // TODO add validators
    public string AddressLine1 { get; set; } = string.Empty; // TODO add validators
    public string? AddressLine2 { get; set; } = string.Empty; // TODO add validators
    public string PostCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;


}
