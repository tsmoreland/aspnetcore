namespace BethanysPieShop.Admin.Domain.Validation;

internal sealed class AddressLineValidator : StringValidator
{
    private static readonly Lazy<AddressLineValidator> s_instance = new(() => new AddressLineValidator());

    /// <inheritdoc />
    public AddressLineValidator()
    {
        Initialize();
    }

    /// <inheritdoc />
    protected override int MinimumLength => 5;

    /// <inheritdoc />
    protected override int MaximumLength => 150;

    /// <inheritdoc />
    protected override bool AllowNull => false;

    public static AddressLineValidator Instance => s_instance.Value;
}
