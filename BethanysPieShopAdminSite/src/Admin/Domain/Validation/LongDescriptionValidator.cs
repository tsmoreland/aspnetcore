namespace BethanysPieShop.Admin.Domain.Validation;

internal sealed class LongDescriptionValidator : NullableStringValidator
{
    private static readonly Lazy<LongDescriptionValidator> s_instance = new(() => new LongDescriptionValidator());

    /// <inheritdoc />
    public LongDescriptionValidator()
    {
        Initialize();
    }
    public static LongDescriptionValidator Instance => s_instance.Value;

    /// <inheritdoc />
    protected override int MinimumLength => 0;

    /// <inheritdoc />
    protected override int MaximumLength => 1000;

    /// <inheritdoc />
    protected override bool AllowNull => true;
}
