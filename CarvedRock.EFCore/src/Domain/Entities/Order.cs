namespace CarvedRock.Domain.Entities;

public sealed class Order
{
    public Order(string name, Customer customer)
        : this(name, customer, new HashSet<Item>())
    {
    }

    public Order(string name, Customer customer, ICollection<Item> items)
        : this(0, name, customer.Id, customer, items)
    {
    }

    private Order(int id, string name, int customerId, Customer customer, ICollection<Item> items)
    {
        Id = id;
        Name = name;
        CustomerId = customerId;
        Customer = customer;
        Items = items;
    }

    public required int Id { get; init; }

    public required string Name { get; init; }

    public required int CustomerId { get; init; }

    public required Customer Customer { get; init; }

    public required ICollection<Item> Items { get; init; }
}
