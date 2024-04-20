namespace MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Requests;

public sealed record class CartItemDto(
    int Id,
    int ProductId,
    string ProductName,
    string? ProductDescription,
    double Price,
    string? ImageUrl,
    int Count);
