namespace MicroShop.Services.ShoppingCart.App.Models.DataTransferObjects;

public sealed record class CartDetailsDto(
    int Id,
    int HeaderId,
    CartHeaderDto Header,
    ProductDto Product,
    int Count);
