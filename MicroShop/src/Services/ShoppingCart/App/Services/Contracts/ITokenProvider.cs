namespace MicroShop.Services.ShoppingCart.App.Services.Contracts;

public interface ITokenProvider
{
    string? GetBearerToken();
}
