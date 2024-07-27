namespace MicroShop.Web.Mvc.App.Services.Contracts;

public interface ITokenProvider
{
    void SetToken(string token);
    string? GetToken();
    void ClearToken();

}
