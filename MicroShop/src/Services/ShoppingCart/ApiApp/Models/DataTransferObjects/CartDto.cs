namespace MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects;

public sealed record class CartDto(CartHeaderDto Header, IEnumerable<CartDetailsDto> Details);
