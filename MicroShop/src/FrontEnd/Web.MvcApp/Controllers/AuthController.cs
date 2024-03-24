using System.Linq.Expressions;
using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Models.Auth;
using MicroShop.Web.MvcApp.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MicroShop.Web.MvcApp.Controllers;

public sealed class AuthController(IAuthService authService) : Controller
{

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginRequestDto());
    }

    [HttpGet]
    public async Task<IActionResult> Register()
    {
        await SetRolelist(ViewBag);
        return View(new RegistrationRequestDto());
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegistrationRequestDto model)
    {
        if (ModelState.IsValid)
        {
            await SetRolelist(ViewBag);
            return View(model);
        }

        ResponseDto<UserDto>? response = await authService.Register(model);
        if (response?.Success != true || response.Data is null)
        {
            await SetRolelist(ViewBag);
            TempData["error"] = "Unable to register";
            return View(model);
        }
        UserDto user = response.Data;

        ResponseDto? assignResponse = await authService.AssignRole(new ChangeRoleDto(model.Email, model.Role ?? string.Empty)); // let auth service handle empty string
        if (response is { Success: true })
        {
            TempData["success"] = "Registration Successful";
            return RedirectToAction(nameof(Login));
        }
        else
        {
            await SetRolelist(ViewBag);
            TempData["error"] = "An error occurred assigning role";
            return View(model);
        }

    }

    public IActionResult Logout()
    {
        return View();
    }

    private async Task SetRolelist(dynamic viewBag)
    {
        viewBag.Rolelist = await GetRoles()
            .Select(role => new SelectListItem(role, role))
            .ToListAsync();
        return;

        async IAsyncEnumerable<string> GetRoles()
        {
            ResponseDto<IEnumerable<string>>? response = await authService.GetRoles();
            if (response?.Data is null)
            {
                yield break;
            }

            foreach (string role in response.Data)
            {
                yield return role;
            }
        }
    }


}
