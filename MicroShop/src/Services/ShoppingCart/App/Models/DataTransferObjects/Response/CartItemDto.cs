namespace MicroShop.Services.ShoppingCart.App.Models.DataTransferObjects.Response;

public sealed record class CartItemDto(
    int Id,
    int ProductId,
    string ProductName,
    string? ProductDescription,
    double Price,
    string? ImageUrl,
    int Count);
