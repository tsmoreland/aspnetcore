namespace MicroShop.Services.Coupons.App.Models;

public sealed class Coupon
{
    private string _code;

    public Coupon(string code, double discountAmount, int minimumAmount)
        : this(0, code, discountAmount, minimumAmount)
    {
    }

    public Coupon(int id, string code, double discountAmount, int minimumAmount)
    {
        Id = id;
        _code = code ?? string.Empty;
        NormalizedCode = _code.ToUpperInvariant();
        DiscountAmount = discountAmount;
        MinimumAmount = minimumAmount;
    }


    public int Id { get; init; }
    public string Code
    {
        get => _code;
        set => SetCode(value);
    }
    public double DiscountAmount { get; set; }
    public int MinimumAmount { get; set; }

    public string NormalizedCode { get; private set; }

    private string SetCode(string? value)
    {
        _code = value ?? string.Empty;
        NormalizedCode = _code.ToUpperInvariant();
        return _code;
    }
}
