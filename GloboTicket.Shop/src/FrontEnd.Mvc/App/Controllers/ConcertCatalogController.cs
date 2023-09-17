using GloboTicket.FrontEnd.Mvc.App.Extensions;
using GloboTicket.FrontEnd.Mvc.App.Models;
using GloboTicket.FrontEnd.Mvc.App.Models.Api;
using GloboTicket.FrontEnd.Mvc.App.Models.View;
using GloboTicket.FrontEnd.Mvc.App.Services.ConcertCatalog;
using GloboTicket.FrontEnd.Mvc.App.Services.ShoppingBasket;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.FrontEnd.Mvc.App.Controllers;

public sealed class ConcertCatalogController : Controller
{
    private readonly IConcertCatalogService _concertCatalogService;
    private readonly IShoppingBasketService _shoppingBasketService;
    private readonly Settings _settings;

    public ConcertCatalogController(IConcertCatalogService concertCatalogService, IShoppingBasketService shoppingBasketService, Settings settings)
    {
        _concertCatalogService = concertCatalogService;
        _shoppingBasketService = shoppingBasketService;
        _settings = settings;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        Guid currentBasketId = Request.Cookies.GetCurrentBasketId(_settings);

        Task<Basket> getBasket = _shoppingBasketService.GetBasket(currentBasketId, cancellationToken);
        Task<List<Concert>> getConcerts = _concertCatalogService.GetAll(cancellationToken).ToListAsync(cancellationToken).AsTask();

        await Task.WhenAll(getBasket, getConcerts);

        int numberOfItems = getBasket.Result.NumberOfItems;

        return View(
            new ConcertListModel
            {
                Concerts = getConcerts.Result,
                NumberOfItems = numberOfItems,
            }
        );
    }

    [HttpGet]
    public async Task<IActionResult> Detail(Guid concertId, CancellationToken cancellationToken)
    {
        Concert? ev = await _concertCatalogService.GetConcert(concertId, cancellationToken);
        return View(ev);
    }
}
