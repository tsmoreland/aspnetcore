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
