using GloboTicket.FrontEnd.Mvc.App.Extensions;
using GloboTicket.FrontEnd.Mvc.App.Models;
using GloboTicket.FrontEnd.Mvc.App.Models.Api;
using GloboTicket.FrontEnd.Mvc.App.Models.View;
using GloboTicket.FrontEnd.Mvc.App.Services.ShoppingBasket;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.FrontEnd.Mvc.App.Controllers;

public class ShoppingBasketController : Controller
{
    private readonly IShoppingBasketService _basketService;
    private readonly Settings _settings;
    private readonly ILogger<ShoppingBasketController> _logger;
    private readonly TelemetryClient _telemetryClient;

    public ShoppingBasketController(IShoppingBasketService basketService, TelemetryClient telemetryClient, Settings settings, ILogger<ShoppingBasketController> logger)
    {
        _basketService = basketService;
        _settings = settings;
        _logger = logger;
        _telemetryClient = telemetryClient;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        IAsyncEnumerable<BasketLine> basketLines = _basketService
            .GetLinesForBasket(Request.Cookies.GetCurrentBasketId(_settings), cancellationToken);

        IAsyncEnumerable<BasketLineViewModel> lineViewModels = basketLines
            .Select(bl => new BasketLineViewModel
            {
                LineId = bl.BasketLineId,
                ConcertId = bl.ConcertId,
                ConcertName = bl.Concert.Name,
                Date = bl.Concert.Date,
                Price = bl.Price,
                Quantity = bl.TicketAmount
            });
        return View(await lineViewModels.ToListAsync(cancellationToken));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddLine(BasketLineForCreation basketLine, CancellationToken cancellationToken)
    {
        SendAppInsightsTelemetryAddLine(basketLine);
        Guid basketId = Request.Cookies.GetCurrentBasketId(_settings);
        BasketLine newLine = await _basketService.AddToBasket(basketId, basketLine, cancellationToken);
        Response.Cookies.Append(_settings.BasketIdCookieName, newLine.BasketId.ToString());

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateLine(BasketLineForUpdate basketLineUpdate, CancellationToken cancellationToken)
    {
        SendAppInsightsTelemetryUpdateLine(basketLineUpdate);
        Guid basketId = Request.Cookies.GetCurrentBasketId(_settings);
        await _basketService.UpdateLine(basketId, basketLineUpdate, cancellationToken);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> RemoveLine(Guid lineId, CancellationToken cancellationToken)
    {
        Guid basketId = Request.Cookies.GetCurrentBasketId(_settings);
        await _basketService.RemoveLine(basketId, lineId, cancellationToken);
        return RedirectToAction("Index");
    }

    private void SendAppInsightsTelemetryAddLine(BasketLineForCreation basketLine)
    {
        MetricTelemetry telemetry = new() { Name = "Items in basket", Sum = basketLine.TicketAmount };
        _telemetryClient.TrackMetric(telemetry);
    }

    private void SendAppInsightsTelemetryUpdateLine(BasketLineForUpdate basketLine)
    {
        MetricTelemetry telemetry = new() { Name = "Items in basket", Sum = basketLine.TicketAmount };
        _telemetryClient.TrackMetric(telemetry);
    }
}
