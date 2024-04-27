namespace MicroShop.Services.Orders.ApiApp.Api.Commands;

public sealed class StripeOptions
{
    internal const string SectionName = nameof(StripeOptions);

    public string ApiKey { get; set; } = string.Empty;

}
