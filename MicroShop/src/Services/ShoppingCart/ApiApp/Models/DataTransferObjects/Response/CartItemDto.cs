namespace MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects.Response;

public sealed record class CartItemDto(
    int Id,
    int ProductId,
    string ProductName,
    double Price,
    string? ImageUrl,
    int Count);
