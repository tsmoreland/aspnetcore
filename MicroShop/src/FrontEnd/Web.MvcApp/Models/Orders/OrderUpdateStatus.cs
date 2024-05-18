namespace MicroShop.Web.MvcApp.Models.Orders;

public enum OrderUpdateStatus
{
    Approved = (int)OrderStatus.Approved,
    Processing,
    WaitingToShip,
    Shipping,
    Complete,
    Cancelled,
}
