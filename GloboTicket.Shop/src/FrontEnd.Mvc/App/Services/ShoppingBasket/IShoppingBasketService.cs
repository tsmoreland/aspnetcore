using GloboTicket.FrontEnd.Mvc.App.Models.Api;

namespace GloboTicket.FrontEnd.Mvc.App.Services.ShoppingBasket;

public interface IShoppingBasketService
{
    Task<BasketLine> AddToBasket(Guid basketId, BasketLineForCreation basketLine, CancellationToken cancellationToken);
    IAsyncEnumerable<BasketLine> GetLinesForBasket(Guid basketId, CancellationToken cancellationToken);
    Task<Basket> GetBasket(Guid basketId, CancellationToken cancellationToken);
    Task UpdateLine(Guid basketId, BasketLineForUpdate basketLineForUpdate, CancellationToken cancellationToken);
    Task RemoveLine(Guid basketId, Guid lineId, CancellationToken cancellationToken);
    Task ClearBasket(Guid currentBasketId, CancellationToken cancellationToken);
}
