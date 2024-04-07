namespace MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects;

public sealed record class CartHeaderDto(
    int Id,
    string? UserId,
    string? CouponCode,
    double Discount,
    double CartTotal);
