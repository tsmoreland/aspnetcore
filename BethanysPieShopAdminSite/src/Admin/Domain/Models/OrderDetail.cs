namespace BethanysPieShop.Admin.Domain.Models;

public sealed class OrderDetail
{
    public Guid Id { get; }
    public Guid OrderId { get; }
    public Guid PieId { get; }
    public int Amount { get; set; }
    public Pie Pie { get; set; } = default!;
    public Order Order { get; set; } = default!;
}
