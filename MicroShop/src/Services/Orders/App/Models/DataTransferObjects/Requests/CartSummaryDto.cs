namespace MicroShop.Services.Orders.App.Models.DataTransferObjects.Requests;

public sealed record CartSummaryDto(
    int Id,
    string? CouponCode,
    double Discount,
    double CartTotal,
    IEnumerable<CartItemDto> Details);
