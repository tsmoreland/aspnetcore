using System.ComponentModel.DataAnnotations;

namespace MicroShop.Services.Auth.App.Models.DataTransferObjects;

public sealed record class RegistrationRequestDto(
    [property: EmailAddress] string Email,
    [property: MinLength(2), MaxLength(200)] string Name,
    [property: Phone] string PhoneNumber,
    [property: MinLength(12)] string Password,
    string ConfirmPassword) : IValidatableObject
{
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
