namespace MicroShop.Services.Email.App.Models.DataTransferObjects;

public sealed record CartSummaryDto(
    int Id,
    string? CouponCode,
    double Discount,
    double CartTotal,
    IEnumerable<CartItemDto> Details)
{
    public static CartSummaryDto Empty() =>
        new(0, null, 0.0, 0.0, []);
}
