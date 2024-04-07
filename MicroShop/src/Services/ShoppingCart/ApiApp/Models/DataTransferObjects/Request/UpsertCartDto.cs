namespace MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects.Request;

public sealed record class UpsertCartDto(
    int? HeaderId,
    int ProductId,
    int Count);
