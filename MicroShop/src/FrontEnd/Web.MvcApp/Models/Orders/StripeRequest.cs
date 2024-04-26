namespace MicroShop.Web.MvcApp.Models.Orders;

public sealed record class StripeRequest(string SessionId, Uri SessionUrl, Uri ApprovedUrl, Uri CancelUrl, OrderSummaryDto Order);
