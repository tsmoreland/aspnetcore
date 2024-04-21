namespace MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Responses;

public sealed record class OrderItemDto(
    int Id,
    int ProductId,
    string ProductName,
    double Price,
    int Count);
