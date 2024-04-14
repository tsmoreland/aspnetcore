namespace MicroShop.Services.Email.ApiApp.Models.DataTransferObjects;

public sealed record class CartItemDto(
    int Id,
    int ProductId,
    string ProductName,
    string? ProductDescription,
    double Price,
    string? ImageUrl,
    int Count);
