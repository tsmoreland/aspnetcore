namespace MicroShop.Services.Orders.App.Models.DataTransferObjects.Requests;

public sealed record class CartItemDto(
    int Id,
    int ProductId,
    string ProductName,
    string? ProductDescription,
    double Price,
    string? ImageUrl,
    int Count);
