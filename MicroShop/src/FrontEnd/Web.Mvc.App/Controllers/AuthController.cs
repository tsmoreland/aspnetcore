using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MicroShop.Web.Mvc.App.Models;
using MicroShop.Web.Mvc.App.Models.Auth;
using MicroShop.Web.Mvc.App.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MicroShop.Web.Mvc.App.Controllers;

public sealed class AuthController(IAuthService authService, ITokenProvider tokenProvider) : Controller
{

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginRequestDto());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestDto model)
    {
        const string errorMessage = "Unable to login username or password is incorrect";
        if (!ModelState.IsValid)
        {
            model.Password = string.Empty;
            return View(model);
        }

        ResponseDto<LoginResponseDto>? response = await authService.Login(model);
        if (response is not { Success: true, Data: not null })
        {
            ModelState.AddModelError(string.Empty, errorMessage);
            model.Password = string.Empty;
            return View(model);
        }

        LoginResponseDto loginResponse = response.Data;
        await SignInUser(loginResponse);
        tokenProvider.SetToken(loginResponse.Token);
        return RedirectToAction("Index", "Home");

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
        if (!ModelState.IsValid)
        {
            await SetRolelist(ViewBag);
            return View(model);
        }

        ResponseDto<UserDto>? response = await authService.Register(model);
        if (response?.Success != true || response.Data is null)
        {
            await SetRolelist(ViewBag);
            ModelState.AddModelError(string.Empty, response?.ErrorMessage ?? "Unable to register");
            return View(model);
        }

        ResponseDto? assignResponse = await authService.AssignRole(new ChangeRoleDto(model.Email, model.Role ?? string.Empty)); // let auth service handle empty string
        if (response is { Success: true })
        {
            return RedirectToAction(nameof(Login));
        }
        else
        {
            await SetRolelist(ViewBag);
            ModelState.AddModelError(string.Empty, assignResponse?.ErrorMessage ?? "An error occurred assigning role");
            return View(model);
        }

    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        tokenProvider.ClearToken();

        return RedirectToAction("Index", "Home");
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

    private async Task SignInUser(LoginResponseDto model)
    {
        JwtSecurityTokenHandler handler = new();
        JwtSecurityToken jwt = handler.ReadJwtToken(model.Token);
        ClaimsIdentity identity = new(CookieAuthenticationDefaults.AuthenticationScheme);
        Dictionary<string, List<string>> claimsByType = jwt.Claims.GroupBy(c => c.Type).ToDictionary(g => g.Key, g => g.Select(pair => pair.Value).ToList());

        if (claimsByType.TryGetValue(JwtRegisteredClaimNames.Email, out List<string>? emailValues))
        {
            string email = emailValues.First();
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, email));
            identity.AddClaim(new Claim(ClaimTypes.Name, email));
        }
        TryAddClaim(identity, JwtRegisteredClaimNames.Sub, claimsByType);
        TryAddClaim(identity, JwtRegisteredClaimNames.Name, claimsByType);
        if (claimsByType.TryGetValue(ClaimTypes.Role, out List<string>? roles))
        {
            foreach (string role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
        }


        ClaimsPrincipal principal = new(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        return;

        static void TryAddClaim(ClaimsIdentity identity, string claimName, IReadOnlyDictionary<string, List<string>> claimsByType)
        {
            if (claimsByType.TryGetValue(claimName, out List<string>? values))
            {
                string value = values.First();
                identity.AddClaim(new Claim(claimName, value));
            }
        }
    }
}
