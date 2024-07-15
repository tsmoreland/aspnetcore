namespace MicroShop.Services.Auth.AuthApiApp.Models;

public class RabbitConnectionSettings(string hostname, string username, string password)
{
    public RabbitConnectionSettings()
        : this("localhost", string.Empty, string.Empty)
    {
    }

    public string Hostname { get; set; } = hostname;
    public string Username { get; set; } = username;
    public string Password { get; set; } = password;
}
