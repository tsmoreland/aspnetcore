namespace MicroShop.Web.Mvc.App.Models.Cart;

public sealed record class CartItemDto(
    int Id,
    int ProductId,
    string ProductName,
    string? ProductDescription,
    double Price,
    string? ImageUrl,
    int Count);
