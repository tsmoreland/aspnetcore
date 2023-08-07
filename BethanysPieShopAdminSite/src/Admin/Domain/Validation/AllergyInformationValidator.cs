namespace BethanysPieShop.Admin.Domain.Validation;

internal sealed class AllergyInformationValidator : NullableStringValidator
{
    private static readonly Lazy<AllergyInformationValidator> s_instance = new(() => new AllergyInformationValidator());

    /// <inheritdoc />
    public AllergyInformationValidator()
    {
        Initialize();
    }

    public static AllergyInformationValidator Instance => s_instance.Value;

    /// <inheritdoc />
    protected override int MinimumLength => 0;

    /// <inheritdoc />
    protected override int MaximumLength => 1000;

    /// <inheritdoc />
    protected override bool AllowNull => true;
}
