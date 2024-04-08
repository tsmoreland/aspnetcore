namespace MicroShop.Web.MvcApp.Models.Cart;

public sealed record class UpsertCartDto(
    int? HeaderId,
    int ProductId,
    int Count);
