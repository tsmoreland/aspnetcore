using MicroShop.Services.Orders.App.Models.DataTransferObjects.Responses;

namespace MicroShop.Services.Orders.App.Models.DataTransferObjects.Requests;

public sealed record class StripeRequestDto(Uri ApprovedUrl, Uri CancelUrl, OrderSummaryDto Order);
