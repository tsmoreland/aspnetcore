namespace MicroShop.Web.MvcApp.Models.Orders;

public enum OrderStatus
{
    Pending,
    Processing,
    WaitingToShip,
    Shipping,
    Complete,
}
