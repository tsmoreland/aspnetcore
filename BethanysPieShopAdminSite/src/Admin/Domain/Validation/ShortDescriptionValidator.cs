namespace BethanysPieShop.Admin.Domain.Validation;

internal sealed class ShortDescriptionValidator : NullableStringValidator
{
    private static readonly Lazy<ShortDescriptionValidator> s_instance = new(() => new ShortDescriptionValidator());

    /// <inheritdoc />
    public ShortDescriptionValidator()
    {
        Initialize();
    }
    public static ShortDescriptionValidator Instance => s_instance.Value;

    /// <inheritdoc />
    protected override int MinimumLength => 0;

    /// <inheritdoc />
    protected override int MaximumLength => 100;

    /// <inheritdoc />
    protected override bool AllowNull => true;
}
