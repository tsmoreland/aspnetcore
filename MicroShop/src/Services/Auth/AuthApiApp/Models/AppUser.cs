using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MicroShop.Services.Auth.AuthApiApp.Models;

public sealed class AppUser : IdentityUser, IValidatableObject
{
    public string Name { get; set; } = string.Empty;

    /// <inheritdoc />
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        throw new NotImplementedException();
    }
}
