using MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Responses;

namespace MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Requests;

public sealed record class StripeRequestDto(string SessionId, Uri SessionUrl, Uri ApprovedUrl, Uri CancelUrl, OrderSummaryDto Order);
