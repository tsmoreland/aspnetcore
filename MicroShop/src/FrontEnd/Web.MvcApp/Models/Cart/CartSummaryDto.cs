namespace MicroShop.Web.MvcApp.Models.Cart;

public sealed record CartSummaryDto(
    int Id,
    string? CouponCode,
    double Discount,
    double CartTotal,
    IEnumerable<CartItemDto> Details);
