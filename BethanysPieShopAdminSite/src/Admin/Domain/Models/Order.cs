using BethanysPieShop.Admin.Domain.Validation;

namespace BethanysPieShop.Admin.Domain.Models;

public sealed class Order
{
    private readonly HashSet<OrderDetail> _orderDetails = new();
    private string _firstName;
    private string _lastName;
    private string _addressLine1;
    private string? _addressLine2;
    private string _postCode;
    private string _city;
    private string _email;
    private string _phoneNumber;


    public Order(OrderStatus orderStatus, string firstName, string lastName, string addressLine1, string? addressLine2, string postCode, string city, string phoneNumber, string email, IEnumerable<OrderDetail> orderDetails)
        : this(Guid.NewGuid(), orderStatus,
              NameValidator.Instance.ValidateOrThrow(firstName),
              NameValidator.Instance.ValidateOrThrow(lastName),
              AddressLineValidator.Instance.ValidateOrThrow(addressLine1),
              NullableAddressLineValidator.Instance.ValidateOrThrow(addressLine2),
              PostCodeValidator.Instance.ValidateOrThrow(postCode),
              NameValidator.Instance.ValidateOrThrow(city),
              PhoneNumberValidator.Instance.ValidateOrThrow(phoneNumber),
              EmailValidator.Instance.ValidateOrThrow(email),
              decimal.Zero,
              DateTime.UtcNow)
    {
        ArgumentNullException.ThrowIfNull(orderDetails);
        List<OrderDetail> items = orderDetails.ToList(); ;
        OrderTotal = CalculateOrderTotal(items);

        foreach (OrderDetail item in items)
        {
            _orderDetails.Add(item);
        }
    }

    private Order(Guid id, OrderStatus orderStatus, string firstName, string lastName, string addressLine1, string? addressLine2, string postCode, string city, string phoneNumber, string email, decimal orderTotal, DateTime orderPlaced)
    {
        Id = id;
        OrderStatus = orderStatus;
        _firstName = firstName;
        _lastName = lastName;
        _addressLine1 = addressLine1;
        _addressLine2 = addressLine2;
        _postCode = postCode;
        _city = city;
        _email = email;
        _phoneNumber = phoneNumber;
        OrderTotal = orderTotal; 
        OrderPlaced = orderPlaced;

    }

    public Guid Id { get; }
    public ICollection<OrderDetail> OrderDetails => _orderDetails.ToList();
    public OrderStatus OrderStatus { get; set; }
    public string FirstName { get => _firstName; set => _firstName = NameValidator.Instance.ValidateOrThrow(value); } 
    public string LastName { get => _lastName; set => _lastName = NameValidator.Instance.ValidateOrThrow(value); }
    public string AddressLine1 { get => _addressLine1; set => _addressLine1 = AddressLineValidator.Instance.ValidateOrThrow(value); }
    public string? AddressLine2 { get => _addressLine2; set => _addressLine2 = NullableAddressLineValidator.Instance.ValidateOrThrow(value); }
    public string PostCode { get => _postCode; set => _postCode = PostCodeValidator.Instance.ValidateOrThrow(value); }
    public string City { get => _city; set => _city = NameValidator.Instance.ValidateOrThrow(value); }

    public string PhoneNumber { get => _phoneNumber; set => _phoneNumber = PhoneNumberValidator.Instance.ValidateOrThrow(value); }

    public string Email { get => _email; set => _email = EmailValidator.Instance.ValidateOrThrow(value); }
    
    public decimal OrderTotal { get; private set; }

    public DateTime OrderPlaced { get; }

    public byte[] ConcurrencyToken { get; set; } = Array.Empty<byte>();

    public void AddOrderLine(int amount, Pie pie)
    {
        ArgumentNullException.ThrowIfNull(pie);
        OrderDetail detail = new(amount, this, pie);
        _orderDetails.Add(detail);
        OrderTotal = CalculateOrderTotal(OrderDetails);
    }

    private static decimal CalculateOrderTotal(IEnumerable<OrderDetail> orderDetails)
    {
        ArgumentNullException.ThrowIfNull(orderDetails);
        return orderDetails.Aggregate(decimal.Zero, (current, line) => decimal.Add(current, decimal.Multiply(line.Price, new decimal(line.Amount))));
    }
}
