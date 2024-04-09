using System.ComponentModel.DataAnnotations;

namespace MicroShop.Web.MvcApp.Models.Auth;

public sealed class RegistrationRequestDto(string email, string name, string phoneNumber, string password, string confirmPassword, string? role) : IValidatableObject
{
    public RegistrationRequestDto()
        : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null)
    {
    }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = email;

    [Required]
    [MinLength(2)]
    [MaxLength(200)]
    public string Name { get; set; } = name;

    [Phone]
    public string PhoneNumber { get; set; } = phoneNumber;

    [Required]
    [MinLength(12)]
    public string Password { get; set; } = password;

    [Required]
    [MinLength(12)]
    public string ConfirmPassword { get; set; } = confirmPassword;

    public string? Role { get; set; } = role;

    /// <inheritdoc />
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> results = [];
        Validator.TryValidateProperty(Email, ContextForProperty(nameof(Email)), results);
        Validator.TryValidateProperty(Name, ContextForProperty(nameof(Name)), results);
        Validator.TryValidateProperty(PhoneNumber, ContextForProperty(nameof(PhoneNumber)), results);
        Validator.TryValidateProperty(Password, ContextForProperty(nameof(Password)), results);

        if (Password != ConfirmPassword)
        {
            results.Add(new ValidationResult("Passwords must match"));
        }

        return results;

        ValidationContext ContextForProperty(string propertyName) =>
            new(this, null, null) { MemberName = propertyName };
    }
}
