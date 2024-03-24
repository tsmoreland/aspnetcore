namespace MicroShop.Web.MvcApp.Services.Contracts;

public interface ITokenProvider
{
    void SetToken(string token);
    string? GetToken();
    void ClearToken();

}
