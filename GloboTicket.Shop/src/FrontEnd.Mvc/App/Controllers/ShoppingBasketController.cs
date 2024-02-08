using GloboTicket.FrontEnd.Mvc.App.Extensions;
using GloboTicket.FrontEnd.Mvc.App.Models;
using GloboTicket.FrontEnd.Mvc.App.Models.Api;
using GloboTicket.FrontEnd.Mvc.App.Models.View;
using GloboTicket.FrontEnd.Mvc.App.Services.ShoppingBasket;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.FrontEnd.Mvc.App.Controllers;

public class ShoppingBasketController(
    IShoppingBasketService basketService,
    TelemetryClient telemetryClient,
    Settings settings,
    ILogger<ShoppingBasketController> logger)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        _ = logger;
        
        IAsyncEnumerable<BasketLine> basketLines = basketService
            .GetLinesForBasket(Request.Cookies.GetCurrentBasketId(settings), cancellationToken);

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
        Guid basketId = Request.Cookies.GetCurrentBasketId(settings);
        BasketLine newLine = await basketService.AddToBasket(basketId, basketLine, cancellationToken);
        Response.Cookies.Append(settings.BasketIdCookieName, newLine.BasketId.ToString());

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateLine(BasketLineForUpdate basketLineUpdate, CancellationToken cancellationToken)
    {
        SendAppInsightsTelemetryUpdateLine(basketLineUpdate);
        Guid basketId = Request.Cookies.GetCurrentBasketId(settings);
        await basketService.UpdateLine(basketId, basketLineUpdate, cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> RemoveLine(Guid lineId, CancellationToken cancellationToken)
    {
        Guid basketId = Request.Cookies.GetCurrentBasketId(settings);
        await basketService.RemoveLine(basketId, lineId, cancellationToken);
        return RedirectToAction("Index");
    }

    private void SendAppInsightsTelemetryAddLine(BasketLineForCreation basketLine)
    {
        MetricTelemetry telemetry = new() { Name = "Items in basket", Sum = basketLine.TicketAmount };
        telemetryClient.TrackMetric(telemetry);
    }

    private void SendAppInsightsTelemetryUpdateLine(BasketLineForUpdate basketLine)
    {
        MetricTelemetry telemetry = new() { Name = "Items in basket", Sum = basketLine.TicketAmount };
        telemetryClient.TrackMetric(telemetry);
    }
}
