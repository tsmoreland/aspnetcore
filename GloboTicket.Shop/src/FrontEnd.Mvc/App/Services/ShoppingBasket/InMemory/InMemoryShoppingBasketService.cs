using System.Runtime.CompilerServices;
using GloboTicket.FrontEnd.Mvc.App.Models;
using GloboTicket.FrontEnd.Mvc.App.Models.Api;
using GloboTicket.FrontEnd.Mvc.App.Services.ConcertCatalog;

namespace GloboTicket.FrontEnd.Mvc.App.Services.ShoppingBasket.InMemory;

public sealed class InMemoryShoppingBasketService : IShoppingBasketService
{
    private readonly Settings _settings;
    private readonly IConcertCatalogService _concertCatalogService;
    private readonly Dictionary<Guid, InMemoryBasket> _baskets;
    private readonly Dictionary<Guid, Concert> _concertsCache;

    /// <summary/>
    public InMemoryShoppingBasketService(Settings settings, IConcertCatalogService concertCatalogService)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _concertCatalogService = concertCatalogService ?? throw new ArgumentNullException(nameof(concertCatalogService));
        _baskets = new Dictionary<Guid, InMemoryBasket>();
        _concertsCache = new Dictionary<Guid, Concert>();
    }

    /// <inheritdoc />
    public async Task<BasketLine> AddToBasket(Guid basketId, BasketLineForCreation basketLine, CancellationToken cancellationToken)
    {
        if (!_baskets.TryGetValue(basketId, out InMemoryBasket? basket))
        {
            basket = new InMemoryBasket(_settings.UserId);
            _baskets.Add(basket.BasketId, basket);
        }

        if (_concertsCache.TryGetValue(basketLine.ConcertId, out Concert? concert))
        {
            return basket.Add(basketLine, concert);
        }

        concert = await _concertCatalogService.GetConcert(basketLine.ConcertId, cancellationToken);
        if (concert is not null)
        {
            _concertsCache.Add(basketLine.ConcertId, concert);
        }
        else
        {
            throw new ApplicationException("Concert not found");
        }

        return basket.Add(basketLine, concert);
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<BasketLine> GetLinesForBasket(Guid basketId, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (!_baskets.TryGetValue(basketId, out InMemoryBasket? basket))
        {
            yield break;
        }

        foreach (BasketLine line in basket.Lines)
        {
            yield return line;
        }
    }

    /// <inheritdoc />
    public async Task<Basket> GetBasket(Guid basketId, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        _baskets.TryGetValue(basketId, out InMemoryBasket? basket);
        return new Basket()
        {
            BasketId = basketId,
            NumberOfItems = basket?.Lines?.Count ?? 0,
            UserId = basket?.UserId ?? Guid.Empty
        };
    }

    /// <inheritdoc />
    public async Task UpdateLine(Guid basketId, BasketLineForUpdate basketLineForUpdate, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        if (_baskets.TryGetValue(basketId, out InMemoryBasket? basket))
        {
            basket.Update(basketLineForUpdate);
        }
    }

    /// <inheritdoc />
    public async Task RemoveLine(Guid basketId, Guid lineId, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        if (_baskets.TryGetValue(basketId, out InMemoryBasket? basket))
        {
            basket.Remove(lineId);
        }
    }

    /// <inheritdoc />
    public async Task ClearBasket(Guid currentBasketId, CancellationToken cancellationToken)
    {
        if (_baskets.TryGetValue(currentBasketId, out InMemoryBasket? basket))
        {
            basket.Clear();
        }
        await Task.CompletedTask;
    }
}
