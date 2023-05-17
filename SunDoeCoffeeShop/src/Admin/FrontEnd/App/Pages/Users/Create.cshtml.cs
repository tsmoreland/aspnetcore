using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SunDoeCoffeeShop.Shared.AuthPersistence;

namespace SunDoeCoffeeShop.Admin.FrontEnd.App.Pages.Users;

public class CreateModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;

    /// <inheritdoc />
    public CreateModel(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    [Required]
    [EmailAddress]
    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [BindProperty]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    [BindProperty]
    public string ConfirmPassword { get; set; } = string.Empty;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        IdentityUser user = new(Email) { Email = Email, EmailConfirmed = true };
        string passwordHash = _userManager.PasswordHasher.HashPassword(user, Password);
        user.PasswordHash = passwordHash;

        IdentityResult result = await _userManager.CreateAsync(user);
        if (result != IdentityResult.Success)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return Page();
        }

        return RedirectToPage("./Index");
    }
}
