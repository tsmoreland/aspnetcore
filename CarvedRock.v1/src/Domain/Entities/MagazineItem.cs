using CarvedRock.Domain.ValueObjects;

namespace CarvedRock.Domain.Entities;

public sealed class MagazineItem : Item
{
    /// <inheritdoc />
    public MagazineItem(string description, decimal price, float weight, PublicationFrequency publicationFrequency)
        : base(description, price, weight)
    {
        PublicationFrequency = publicationFrequency;
    }

    /// <inheritdoc />
    public MagazineItem(string description, decimal price, float weight, ICollection<ItemOrder> orders, PublicationFrequency publicationFrequency)
        : base(description, price, weight, orders)
    {
        PublicationFrequency = publicationFrequency;
    }

    /// <inheritdoc />
    private MagazineItem(int id, string description, decimal price, float weight, ICollection<ItemOrder> itemOrders, PublicationFrequency publicationFrequency)
        : base(id, description, price, weight, itemOrders)
    {
        PublicationFrequency = publicationFrequency;
    }

    public required PublicationFrequency PublicationFrequency { get; init; }
}
