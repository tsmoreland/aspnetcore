namespace MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects;

public sealed record class CartDetailsDto(
    int Id,
    int HeaderId,
    CartHeaderDto Header,
    ProductDto Product,
    int Count);
