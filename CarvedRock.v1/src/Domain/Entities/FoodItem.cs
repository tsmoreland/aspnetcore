using CarvedRock.Domain.ValueObjects;

namespace CarvedRock.Domain.Entities;

public abstract class FoodItem : Item
{
    /// <inheritdoc />
    protected FoodItem(string description, decimal price, float weight, DateTime expiryDate)
        : base(description, price, weight)
    {
        ExpiryDate = expiryDate;
    }

    /// <inheritdoc />
    protected FoodItem(string description, decimal price, float weight, ICollection<ItemOrder> orders, DateTime expiryDate)
        : base(description, price, weight, orders)
    {
        ExpiryDate = expiryDate;
    }

    /// <inheritdoc />
    protected FoodItem(int id, string description, decimal price, float weight, ICollection<ItemOrder> itemOrders, DateTime expiryDate)
        : base(id, description, price, weight, itemOrders)
    {
        ExpiryDate = expiryDate;
    }

    public required DateTime ExpiryDate { get; init; }
}

public sealed class CannedFoodItem : FoodItem
{
    /// <inheritdoc />
    public CannedFoodItem(string description, decimal price, float weight, DateTime expiryDate, CanningMaterial canningMaterial, DateTime productionDate)
        : base(description, price, weight, expiryDate)
    {
        CanningMaterial = canningMaterial;
        ProductionDate = productionDate;
    }

    /// <inheritdoc />
    public CannedFoodItem(string description, decimal price, float weight, ICollection<ItemOrder> orders, DateTime expiryDate, CanningMaterial canningMaterial, DateTime productionDate)
        : base(description, price, weight, orders, expiryDate)
    {
        CanningMaterial = canningMaterial;
        ProductionDate = productionDate;
    }

    /// <inheritdoc />
    public CannedFoodItem(int id, string description, decimal price, float weight, ICollection<ItemOrder> itemOrders, DateTime expiryDate, CanningMaterial canningMaterial, DateTime productionDate)
        : base(id, description, price, weight, itemOrders, expiryDate)
    {
        CanningMaterial = canningMaterial;
        ProductionDate = productionDate;
    }

    public required CanningMaterial CanningMaterial { get; init; }
    public required DateTime ProductionDate { get; init; }
}
