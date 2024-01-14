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
#pragma warning disable IDE0051 // Remove unused private members -- used by EF Core
    private ClothesItem(int id, string description, decimal price, float weight, ICollection<ItemOrder> itemOrders, ClothesFabric fabric)
        : base(id, description, price, weight, itemOrders) => Fabric = fabric;
#pragma warning restore IDE0051 // Remove unused private members

    public required ClothesFabric Fabric { get; init; }
}
