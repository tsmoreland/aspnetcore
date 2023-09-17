using CarvedRock.Domain.ValueObjects;

namespace CarvedRock.Domain.Entities;

public sealed class ClothesItem : Item
{
    /// <inheritdoc />
    public ClothesItem(string description, decimal price, float weight, ClothesFabric fabric)
        : base(description, price, weight)
    {
        Fabric = fabric;
    }

    /// <inheritdoc />
    public ClothesItem(string description, decimal price, float weight, ICollection<ItemOrder> orders, ClothesFabric fabric)
        : base(description, price, weight, orders)
    {
        Fabric = fabric;
    }

    /// <inheritdoc />
    private ClothesItem(int id, string description, decimal price, float weight, ICollection<ItemOrder> itemOrders, ClothesFabric fabric)
        : base(id, description, price, weight, itemOrders)
    {
        Fabric = fabric;
    }

    public required ClothesFabric Fabric { get; init; }
}
