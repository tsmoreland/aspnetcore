using GloboTicket.FrontEnd.Mvc.App.Models.Api;

namespace GloboTicket.FrontEnd.Mvc.App.Services.ShoppingBasket.InMemory;

internal sealed class InMemoryBasket
{
    public Guid BasketId { get; }
    public List<BasketLine> Lines { get; }
    public Guid UserId { get; }


    public InMemoryBasket(Guid userId)
    {
        BasketId = Guid.NewGuid();
        Lines = new List<BasketLine>();
        UserId = userId;
    }
    public BasketLine Add(BasketLineForCreation line, Concert concert)
    {
        BasketLine basketLine = new BasketLine()
        {
            ConcertId = line.Id,
            TicketAmount = line.TicketAmount,
            Concert = concert,
            BasketId = BasketId,
            BasketLineId = Guid.NewGuid(),
            Price = line.Price
        };
        Lines.Add(basketLine);
        return basketLine;
    }
    public void Remove(Guid lineId)
    {
        int index = Lines.FindIndex(bl => bl.BasketLineId == lineId);
        if (index >= 0) Lines.RemoveAt(index);
    }

    public void Update(BasketLineForUpdate basketLineForUpdate)
    {
        int index = Lines.FindIndex(bl => bl.BasketLineId == basketLineForUpdate.LineId);
        Lines[index].TicketAmount = basketLineForUpdate.TicketAmount;
    }
    public void Clear()
    {
        Lines.Clear();
    }
}
