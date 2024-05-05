namespace MicroShop.Web.MvcApp.Models.Orders;

public enum OrderStatus
{
    Pending,
    Approved,
    Processing,
    WaitingToShip,
    Shipping,
    Complete,
}
