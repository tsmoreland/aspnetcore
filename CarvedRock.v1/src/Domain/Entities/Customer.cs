using CarvedRock.Domain.ValueObjects;

namespace CarvedRock.Domain.Entities;

public sealed class Customer
{
    private readonly Dictionary<string, object?> _data = new();

    public Customer(string firstName, string lastName, string username, Address shipToAddress, Address billToAddress)
        : this(firstName, lastName, username, shipToAddress, billToAddress, new HashSet<Order>())
    {
    }

    public Customer(string firstName, string lastName, string username, Address shipToAddress, Address billToAddress, ICollection<Order> orders)
        : this(0, firstName, lastName, username)
    {
        ShipToAddress = shipToAddress;
        BillToAddress = billToAddress;
        Orders = orders;
    }

    private Customer(int id, string firstName, string lastName, string username)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
    }

    public required int Id { get; init; }

    public required string FirstName { get; set; } 

    public required string LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";
    public string OrderedFullName => $"{LastName}, {FirstName}";

    public string Username { get; private set; }

    public required Address ShipToAddress { get; set; }
    public required Address BillToAddress { get; set; }

    public required ICollection<Order> Orders { get; init; }

    public DateTime LastUpdated => (DateTime)(this["LastUpdatedBacking"] ?? throw new KeyNotFoundException("Missing Last Updated"));
    public string ConsumerType => (string)(this["ConsumerTypeBacking"] ?? throw new KeyNotFoundException("Missing Last Updated"));

    public object? this[string key]
    {
        get => _data[key];
        private set => _data[key] = value;
    }

    public void SetUsernameOrThrow(string username)
    {
        const string allowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

        if (username.Any(ch => !allowedCharacters.Contains(ch)))
        {
            throw new ArgumentException("username contains one or more illegal characters");
        }

        Username = username;
    }

    public void UpdateLastUpdated()
    {
        this["LastUpdatedBacking"] = DateTime.UtcNow;
    }

    public void SetConsumerTypeOrThrow(string consumerType)
    {
        if (consumerType is not { Length: > 0 })
        {
            throw new ArgumentException("Consumer type cannot be null or empty");
        }
        this["ConsumerTypeBacking"] = consumerType;
    }
}
