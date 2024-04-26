using MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Requests;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using Microsoft.Extensions.Primitives;

namespace MicroShop.Services.Orders.ApiApp.Api.Commands;

internal sealed class CreateStripeSession
{
    private static class SessionModes
    {
        public const string Payment = "payment";
    }

    public static async Task<IResult> Handle([FromBody] StripeRequestDto request, [FromServices] IConfiguration configuration)
    {
        StripeConfiguration.ApiKey = configuration["StripeApiKey"];
        SessionCreateOptions options = new()
        {
            SuccessUrl = request.ApprovedUrl.ToString(),
            CancelUrl = request.CancelUrl.ToString(),
            LineItems = [],
            Mode = SessionModes.Payment,
        };

        // TODO: move this to a separate factory, it's beyond the scope of this API handler
        throw new NotImplementedException();

    }

}

public sealed class StripeSessionOptions
{
    internal const string SectionName = nameof(StripeSessionOptions);

    public string ApiKey { get; set; } = string.Empty;

}
