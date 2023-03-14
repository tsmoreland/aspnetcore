namespace CarvedRock.Domain.Entities;

public sealed class Item
{
    public Item(string description, decimal price, float weight)
        : this(description, price, weight, new HashSet<ItemOrder>())
    {
    }

    public Item(string description, decimal price, float weight, ICollection<ItemOrder> orders)
        : this(0, description, price, weight, orders)
    {
    }

    private Item(int id, string description, decimal price, float weight, ICollection<ItemOrder> itemOrders)
    {
        Id = id;
        Description = description;
        Price = price;
        Weight = weight;
        ItemOrders = itemOrders;
    }

    public required int Id { get; init; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public required decimal PriceAfterVat { get; init; }
    public required float Weight { get; set; }

    public required ICollection<ItemOrder> ItemOrders { get; init; } 
}
