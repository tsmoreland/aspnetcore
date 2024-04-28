using MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Responses;

namespace MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Requests;

public sealed record class StripeRequestDto(Uri ApprovedUrl, Uri CancelUrl, OrderSummaryDto Order);
