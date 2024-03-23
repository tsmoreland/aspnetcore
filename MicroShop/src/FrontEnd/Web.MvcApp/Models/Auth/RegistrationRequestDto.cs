using System.ComponentModel.DataAnnotations;

namespace MicroShop.Web.MvcApp.Models.Auth;

public sealed record class RegistrationRequestDto(
    [property: EmailAddress] string Email,
    [property: MinLength(2), MaxLength(10)] string Name,
    [property: Phone] string PhoneNumber,
    [property: MinLength(12)] string Password,
    string ConfirmPassword) : IValidatableObject
{
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
