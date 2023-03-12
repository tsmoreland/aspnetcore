using CarvedRock.Domain.ValueObjects;

namespace CarvedRock.Domain.Entities;

public sealed class Order
{
    public Order(string name, Customer customer, Status status)
        : this(name, customer, status, new HashSet<Item>())
    {
    }

    public Order(string name, Customer customer, Status status, ICollection<Item> items)
        : this(0, name, customer.Id, status)
    {
        Customer = customer;
        Items = items;
    }

    private Order(int id, string name, int customerId, Status status)
    {
        Id = id;
        Name = name;
        CustomerId = customerId;
        Status = status;
    }

    public required int Id { get; init; }

    public required string Name { get; init; }

    public required int CustomerId { get; init; }

    public required Customer Customer { get; init; }
    public Status Status { get; set; }

    public required ICollection<Item> Items { get; init; }
}
