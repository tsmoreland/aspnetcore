using System.ComponentModel.DataAnnotations;

namespace MicroShop.Web.MvcApp.Models.Auth;

public sealed class LoginRequestDto(string username, string password)
{
    public LoginRequestDto()
        : this(string.Empty, string.Empty)
    {
    }

    [Required]
    public string UserName { get; set; } = username ?? string.Empty;

    [Required]
    public string Password { get; set; } = password ?? string.Empty;
}
