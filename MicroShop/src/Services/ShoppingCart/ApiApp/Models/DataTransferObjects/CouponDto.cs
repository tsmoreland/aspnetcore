namespace MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects;

public sealed record class CouponDto(int Id, string Code, double DiscountAmount, int MinimumAmount);
