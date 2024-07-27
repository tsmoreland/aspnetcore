namespace MicroShop.Web.Mvc.App.Models.Cart;

public sealed record class UpsertCartDto(
    int? HeaderId,
    int ProductId,
    int Count);
