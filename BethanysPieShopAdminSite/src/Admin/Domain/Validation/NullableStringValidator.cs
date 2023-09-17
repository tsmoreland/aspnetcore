using FluentValidation;

namespace BethanysPieShop.Admin.Domain.Validation;

internal abstract class NullableStringValidator : AbstractValidator<string?>
{
    protected abstract int MinimumLength { get; }
    protected abstract int MaximumLength { get; }
    protected abstract bool AllowNull { get; }
    protected virtual string ErrorMessage => $"Value cannot be empty and must be between {MinimumLength} and {MaximumLength} characters in length.";

    protected void Initialize()
    {
        IRuleBuilderInitial<string?, string?>? builder = RuleFor<string?>(value => value);
        if (!AllowNull)
        {
            builder.Must(value => !string.IsNullOrWhiteSpace(value))
                .MinimumLength(MinimumLength)
                .MaximumLength(MaximumLength)
                .WithMessage(ErrorMessage);
        }
    }
}