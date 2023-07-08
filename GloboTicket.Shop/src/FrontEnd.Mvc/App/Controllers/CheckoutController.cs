using GloboTicket.FrontEnd.Mvc.App.Extensions;
using GloboTicket.FrontEnd.Mvc.App.Models;
using GloboTicket.FrontEnd.Mvc.App.Models.View;
using GloboTicket.FrontEnd.Mvc.App.Services.Ordering;
using GloboTicket.FrontEnd.Mvc.App.Services.ShoppingBasket;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.FrontEnd.Mvc.App.Controllers;

public class CheckoutController : Controller
{
    private readonly IShoppingBasketService _shoppingBasketService;
    private readonly IOrderSubmissionService _orderSubmissionService;
    private readonly Settings _settings;
    private readonly ILogger<CheckoutController> _logger;
    private readonly TelemetryClient _telemetryClient;

    public CheckoutController(IShoppingBasketService shoppingBasketService,
        IOrderSubmissionService orderSubmissionService,TelemetryClient telemetry,
        Settings settings, ILogger<CheckoutController> logger)
    {
        _shoppingBasketService = shoppingBasketService;
        _orderSubmissionService = orderSubmissionService;
        _settings = settings;
        _logger = logger;
        _telemetryClient = telemetry;
    }

    [HttpGet]
    public IActionResult Index()
    {
        Guid currentBasketId = Request.Cookies.GetCurrentBasketId(_settings);

        return View(new CheckoutViewModel() { BasketId = currentBasketId });
    }

    [HttpGet]
    public IActionResult Thanks()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Purchase(CheckoutViewModel checkout, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            Guid currentBasketId = Request.Cookies.GetCurrentBasketId(_settings);
            checkout.BasketId = currentBasketId;

            _logger.LogInformation("Received an order from {CheckoutName}", checkout.Name);
            SendAppInsightsTelemetryOrderPlaced();

            Guid orderId = await _orderSubmissionService.SubmitOrder(checkout, cancellationToken);
            await _shoppingBasketService.ClearBasket(currentBasketId, cancellationToken);

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
        _telemetryClient.TrackMetric(telemetry);
    }

}
