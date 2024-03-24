using System.ComponentModel.DataAnnotations;

namespace MicroShop.Web.MvcApp.Models.Auth;

public sealed class RegistrationRequestDto(string email, string name, string phoneNumber, string password, string confirmPassword) : IValidatableObject
{
    public RegistrationRequestDto()
        : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty)
    {
    }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = email;

    [Required]
    [MinLength(2)]
    [MaxLength(10)]
    public string Name { get; set; } = name;

    [Phone]
    public string PhoneNumber { get; set; } = phoneNumber;

    [Required]
    [MinLength(12)]
    public string Password { get; set; } = password;

    [Required]
    [MinLength(12)]
    public string ConfirmPassword { get; set; } = confirmPassword;

    /// <inheritdoc />
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> results = [];
        Validator.TryValidateProperty(Email, validationContext, results);
        Validator.TryValidateProperty(Name, validationContext, results);
        Validator.TryValidateProperty(PhoneNumber, validationContext, results);
        Validator.TryValidateProperty(Password, validationContext, results);

        if (Password != ConfirmPassword)
        {
            results.Add(new ValidationResult("Passwords must match"));
        }

        return results;
    }
}
