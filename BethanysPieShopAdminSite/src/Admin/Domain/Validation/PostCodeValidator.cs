using FluentValidation;

namespace BethanysPieShop.Admin.Domain.Validation;

internal sealed class PostCodeValidator : AbstractValidator<string>
{
    private static readonly Lazy<PostCodeValidator> s_instance = new(() => new PostCodeValidator());

    /// <inheritdoc />
    public PostCodeValidator()
    {
        // TODO: add regex to match post code
        RuleFor<string>(value => value)
            .Must(v => !string.IsNullOrWhiteSpace(v))
            .WithMessage("Must be a valid postcode");
    }
    public static PostCodeValidator Instance => s_instance.Value;
}