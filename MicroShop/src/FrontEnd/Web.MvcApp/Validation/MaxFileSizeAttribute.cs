using System.ComponentModel.DataAnnotations;

namespace MicroShop.Web.MvcApp.Validation;

public class MaxFileSizeAttribute(int maximumFileSizeInMegaBytes) : ValidationAttribute
{

    /// <inheritdoc />
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file)
        {
            return ValidationResult.Success;
        }

        if (file.Length > maximumFileSizeInMegaBytes * 1024 * 1024)
        {
            return new ValidationResult("Image is too large");
        }

        return ValidationResult.Success;
    }
}
