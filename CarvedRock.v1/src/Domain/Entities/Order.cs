using CarvedRock.Domain.ValueObjects;

namespace CarvedRock.Domain.Entities;

public sealed class Order
{
    public Order(string name, Customer customer, Status status)
        : this(name, customer, status, new HashSet<ItemOrder>())
    {
    }

    public Order(string name, Customer customer, Status status, ICollection<ItemOrder> itemOrders)
        : this(0, name, customer.Id, status)
    {
        Customer = customer;
        ItemOrders = itemOrders;
    }

    private Order(int id, string name, int customerId, Status status)
    {
        Id = id;
        Name = name;
        CustomerId = customerId;
        Status = status;
        ItemOrders = new HashSet<ItemOrder>();
    }

    public required int Id { get; init; }

    public required string Name { get; init; }

    public required int CustomerId { get; init; }

    public required Customer Customer { get; init; }
    public Status Status { get; set; }

    public required ICollection<ItemOrder> ItemOrders { get; init; }
}
