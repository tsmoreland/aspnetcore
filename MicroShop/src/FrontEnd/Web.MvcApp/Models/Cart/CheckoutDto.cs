namespace MicroShop.Web.MvcApp.Models.Cart;

public sealed record CheckoutDto(
    int Id,
    string Name,
    string Email,
    string? CouponCode,
    double Discount,
    double CartTotal,
    IEnumerable<CartItemDto> Details)
{
    public static CheckoutDto CreateNew() =>
        new(0, string.Empty, string.Empty, null, 0.0, 0.0, []);

    public CheckoutDto(string name, string email, CartSummaryDto summary)
        : this(summary.Id, name, email, summary.CouponCode, summary.CartTotal, summary.Discount, summary.Details.ToList())
    {
    }

}
