namespace CarvedRock.Domain.Entities;

public sealed class Customer
{
    public Customer(string firstName, string lastName, string username)
        : this(0, firstName, lastName, username, new HashSet<Order>())
    {
    }

    private Customer(int id, string firstName, string lastName, string username, ICollection<Order> orders)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Orders = orders;
    }

    public required int Id { get; init; }

    public required string FirstName { get; set; } 

    public required string LastName { get; set; }

    public required string Username { get; set; }

    public required ICollection<Order> Orders { get; init; } 

}
