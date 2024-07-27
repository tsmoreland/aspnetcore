namespace MicroShop.Web.Mvc.App.Models.Cart;

public sealed record CartSummaryDto(
    int Id,
    string? CouponCode,
    double Discount,
    double CartTotal,
    IEnumerable<CartItemDto> Details)
{
    public static CartSummaryDto CreateNew() =>
        new(0, null, 0.0, 0.0, []);

    public bool IsNewCart => Id is 0;
}
