namespace BethanysPieShop.Admin.Domain.Validation;

internal sealed class NullableAddressLineValidator : NullableStringValidator
{
    private static readonly Lazy<NullableAddressLineValidator> s_instance = new(() => new NullableAddressLineValidator());

    /// <inheritdoc />
    public NullableAddressLineValidator()
    {
        Initialize();
    }

    /// <inheritdoc />
    protected override int MinimumLength => 5;

    /// <inheritdoc />
    protected override int MaximumLength => 150;

    /// <inheritdoc />
    protected override bool AllowNull => true;

    public static NullableAddressLineValidator Instance => s_instance.Value;
}
