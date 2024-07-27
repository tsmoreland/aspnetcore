namespace MicroShop.Services.ShoppingCart.App.Models.DataTransferObjects.Request;

public sealed record class UpsertCartDto(
    int? HeaderId,
    int ProductId,
    int Count);
