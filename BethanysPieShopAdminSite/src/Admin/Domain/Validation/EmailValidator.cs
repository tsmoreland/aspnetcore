using FluentValidation;

namespace BethanysPieShop.Admin.Domain.Validation;

internal sealed class EmailValidator : AbstractValidator<string>
{
    private static readonly Lazy<EmailValidator> s_instance = new(() => new EmailValidator());

    /// <inheritdoc />
    public EmailValidator()
    {
        RuleFor<string>(value => value)
            .EmailAddress()
            .MaximumLength(150)
            .Must(v => !string.IsNullOrWhiteSpace(v))
            .WithMessage("Must be a valid e-mail addresss");
    }
    public static EmailValidator Instance => s_instance.Value;
}
