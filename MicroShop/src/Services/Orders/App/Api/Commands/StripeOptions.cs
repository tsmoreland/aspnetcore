namespace MicroShop.Services.Orders.App.Api.Commands;

public sealed class StripeOptions
{
    internal const string SectionName = nameof(StripeOptions);

    public string ApiKey { get; set; } = string.Empty;

}
