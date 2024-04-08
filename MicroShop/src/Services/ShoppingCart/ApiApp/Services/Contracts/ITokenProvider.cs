namespace MicroShop.Services.ShoppingCart.ApiApp.Services.Contracts;

public interface ITokenProvider
{
    string? GetBearerToken();
}
