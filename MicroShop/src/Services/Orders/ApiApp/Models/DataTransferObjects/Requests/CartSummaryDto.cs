namespace MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Requests;

public sealed record CartSummaryDto(
    int Id,
    string? CouponCode,
    double Discount,
    double CartTotal,
    IEnumerable<CartItemDto> Details);
