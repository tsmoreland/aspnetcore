namespace CarvedRock.Domain.Entities;

public sealed class DriedFoodItem : FoodItem
{
    /// <inheritdoc />
    public DriedFoodItem(string description, decimal price, float weight, DateTime expiryDate, DateTime productionDate)
        : base(description, price, weight, expiryDate)
    {
        ProductionDate = productionDate;
    }

    /// <inheritdoc />
    public DriedFoodItem(string description, decimal price, float weight, ICollection<ItemOrder> orders, DateTime expiryDate, DateTime productionDate)
        : base(description, price, weight, orders, expiryDate)
    {
        ProductionDate = productionDate;
    }

    /// <inheritdoc />
    public DriedFoodItem(int id, string description, decimal price, float weight, ICollection<ItemOrder> itemOrders, DateTime expiryDate, DateTime productionDate)
        : base(id, description, price, weight, itemOrders, expiryDate)
    {
        ProductionDate = productionDate;
    }

    public required DateTime ProductionDate { get; init; }
}
