namespace MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects.Response;

public sealed record class CartItemDto(
    int Id,
    int ProductId,
    string ProductName,
    string? ProductDescription,
    double Price,
    string? ImageUrl,
    int Count);
