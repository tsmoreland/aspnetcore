using GloboTicket.FrontEnd.Mvc.App.Extensions;
using GloboTicket.FrontEnd.Mvc.App.Models;
using GloboTicket.FrontEnd.Mvc.App.Models.View;
using GloboTicket.FrontEnd.Mvc.App.Services.Ordering;
using GloboTicket.FrontEnd.Mvc.App.Services.ShoppingBasket;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.FrontEnd.Mvc.App.Controllers;

public class CheckoutController(
    IShoppingBasketService shoppingBasketService,
    IOrderSubmissionService orderSubmissionService,
    TelemetryClient telemetryClient,
    Settings settings,
    ILogger<CheckoutController> logger)
    : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        Guid currentBasketId = Request.Cookies.GetCurrentBasketId(settings);

        return View(new CheckoutViewModel() { BasketId = currentBasketId });
    }

    [HttpGet]
    public IActionResult Thanks()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Purchase(CheckoutViewModel checkout, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            Guid currentBasketId = Request.Cookies.GetCurrentBasketId(settings);
            checkout.BasketId = currentBasketId;

            logger.LogInformation("Received an order from {CheckoutId}", currentBasketId);
            SendAppInsightsTelemetryOrderPlaced();

            Guid orderId = await orderSubmissionService.SubmitOrder(checkout, cancellationToken);
            await shoppingBasketService.ClearBasket(currentBasketId, cancellationToken);
            _ = orderId;

            return RedirectToAction("Thanks");
        }
        else
        {
            return View("Index");
        }
    }
    private void SendAppInsightsTelemetryOrderPlaced()
    {
        MetricTelemetry telemetry = new() { Name = "Order Placed", Sum = Random.Shared.Next(600) };
        telemetryClient.TrackMetric(telemetry);
    }
}
