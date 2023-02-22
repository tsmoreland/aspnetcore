using GloboTicket.FrontEnd.Mvc.App.Models;

namespace GloboTicket.FrontEnd.Mvc.App.Extensions;

public static class RequestCookieCollectionExtensions
{
    public static Guid GetCurrentBasketId(this IRequestCookieCollection cookies, Settings settings)
    {
        Guid.TryParse(cookies[settings.BasketIdCookieName], out Guid basketId);
        return basketId;
    }
}
