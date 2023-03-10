namespace CarvedRock.Domain.Entities;

public sealed class Item
{
    public Item(string description, decimal price, float weight)
        : this(description, price, weight, new HashSet<Order>())
    {
    }

    public Item(string description, decimal price, float weight, ICollection<Order> orders)
        : this(0, description, price, weight, orders)
    {
    }

    private Item(int id, string description, decimal price, float weight, ICollection<Order> orders)
    {
        Id = id;
        Description = description;
        Price = price;
        Weight = weight;
        Orders = orders;
    }

    public required int Id { get; init; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public required float Weight { get; set; }

    public ICollection<Order> Orders { get; init; } 
}
