namespace MicroShop.Web.Mvc.App.Models.Orders;

public enum OrderStatus
{
    Pending,
    Approved,
    Processing,
    WaitingToShip,
    Shipping,
    Complete,
    Cancelled,
}
