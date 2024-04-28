namespace MicroShop.Web.MvcApp.Models.Orders;

public sealed record class StripeRequest(Uri ApprovedUrl, Uri CancelUrl, OrderSummaryDto Order);
