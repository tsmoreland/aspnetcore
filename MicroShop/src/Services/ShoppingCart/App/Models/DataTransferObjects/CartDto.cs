namespace MicroShop.Services.ShoppingCart.App.Models.DataTransferObjects;

public sealed record class CartDto(CartHeaderDto Header, IEnumerable<CartDetailsDto> Details);
