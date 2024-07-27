namespace MicroShop.Web.Mvc.App.Models.Orders;

public sealed record class StripeRequest(Uri ApprovedUrl, Uri CancelUrl, OrderSummaryDto Order);
