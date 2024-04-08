namespace MicroShop.Web.MvcApp.Models.Cart;

public sealed record class CartItemDto(
    int Id,
    int ProductId,
    string ProductName,
    double Price,
    string? ImageUrl,
    int Count);
