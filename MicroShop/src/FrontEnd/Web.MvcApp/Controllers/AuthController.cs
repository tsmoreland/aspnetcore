using MicroShop.Web.MvcApp.Models.Auth;
using MicroShop.Web.MvcApp.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MicroShop.Web.MvcApp.Controllers;

public sealed class AuthController(IAuthService authService) : Controller
{

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginRequestDto());
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegistrationRequestDto());
    }

    public IActionResult Logout()
    {
        return View();
    }
}
