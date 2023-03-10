namespace CarvedRock.Domain.Entities;

public sealed class Order
{
    public Order(string name, Customer customer)
        : this(name, customer, new HashSet<Item>())
    {
    }

    public Order(string name, Customer customer, ICollection<Item> items)
        : this(0, name, customer.Id)
    {
        Customer = customer;
        Items = items;
    }

    private Order(int id, string name, int customerId)
    {
        Id = id;
        Name = name;
        CustomerId = customerId;
    }

    public required int Id { get; init; }

    public required string Name { get; init; }

    public required int CustomerId { get; init; }

    public required Customer Customer { get; init; } 

    public required ICollection<Item> Items { get; init; }
}
