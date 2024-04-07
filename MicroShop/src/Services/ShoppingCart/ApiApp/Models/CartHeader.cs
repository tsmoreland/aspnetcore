using System.Diagnostics.CodeAnalysis;

namespace MicroShop.Services.ShoppingCart.ApiApp.Models;

public sealed class CartHeader(int id, string? userId, string? couponCode, double discount, double cartTotal)
{
    private string? _couponCode = couponCode;

    public CartHeader()
        :this(0, null, null, 0.0, 0.0)
    {
    }

    [SetsRequiredMembers]
    public CartHeader(string? userId, string? couponCode, double discount, double cartTotal)
        : this(0, userId, couponCode, discount, cartTotal)
    {
    }

    public required int Id { get; init; } = id;
    public string? UserId { get; init; } = userId;
    public string? CouponCode
    {
        get => _couponCode;
        set => UpdateCouponCode(value);
    }
    public string? NormalizedCouponCode { get; set; }
    public double Discount { get; set; } = discount;
    public double CartTotal { get; set; } = cartTotal;

    private string? UpdateCouponCode(string? value)
    {
        NormalizedCouponCode = value?.ToUpperInvariant();
        _couponCode = value;
        return value;
    }
}
