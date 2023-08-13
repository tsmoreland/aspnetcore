using BethanysPieShop.Admin.Domain.Validation;

namespace BethanysPieShop.Admin.Domain.Models;

public sealed class OrderDetail
{
    private int _amount;
    private Pie _pie = default!;

    internal OrderDetail(int amount, Order order, Pie pie)
        : this(
              Guid.NewGuid(),
              order?.Id ?? throw new ArgumentNullException(nameof(order)),
              pie?.Id ?? throw new ArgumentNullException(nameof(pie)),
              amount,
              CalculatePrice(amount, pie))
    {
    }

    private OrderDetail(Guid id, Guid orderId, Guid pieId, int amount, decimal price)
    {
        Id = id;
        OrderId = orderId;
        PieId = pieId;
        _amount = amount;
        Price = price;
    }

    public Guid Id { get; }
    public Guid OrderId { get; }
    public Guid PieId { get; }
    public int Amount
    {
        get => _amount;
        set
        {
            _amount = AmountValidator.Instance.ValidateOrThrow(value);
            Price = CalculatePrice(value, Pie);
        }
    }
    public decimal Price { get; private set; } 
    public Pie Pie
    {
        get => _pie;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _pie = value;
            Price = CalculatePrice(Amount, value);
        }
    } 
    public Order Order { get; set; } = default!;

    public byte[] ConcurrencyToken { get; set; } = Array.Empty<byte>();

    private static decimal CalculatePrice(int amount, Pie pie)
    {
        return decimal.Multiply(pie.Price, Convert.ToDecimal(amount));
    }
}
