using BethanysPieShop.Admin.Domain.Validation;

namespace BethanysPieShop.Admin.Domain.Models;

public sealed class OrderDetail
{
    private int _amount;
    private decimal _price;

    internal OrderDetail(int amount, decimal price, Order order, Pie pie)
        : this(
              Guid.NewGuid(),
              order?.Id ?? throw new ArgumentNullException(nameof(order)),
              pie?.Id ?? throw new ArgumentNullException(nameof(pie)),
              amount,
              CurrencyValidator.Instance.ValidateOrThrow(price))
    {
    }

    private OrderDetail(Guid id, Guid orderId, Guid pieId, int amount, decimal price)
    {
        Id = id;
        OrderId = orderId;
        PieId = pieId;
        _amount = amount;
        _price = price;
    }

    public Guid Id { get; }
    public Guid OrderId { get; }
    public Guid PieId { get; }
    public int Amount { get => _amount; set => _amount = AmountValidator.Instance.ValidateOrThrow(value); }
    public decimal Price { get => _price; set => _price = CurrencyValidator.Instance.ValidateOrThrow(value); }
    public Pie Pie { get; set; } = default!;
    public Order Order { get; set; } = default!;
}
