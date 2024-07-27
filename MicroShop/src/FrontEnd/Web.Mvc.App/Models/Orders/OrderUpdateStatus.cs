namespace MicroShop.Web.Mvc.App.Models.Orders;

public enum OrderUpdateStatus
{
    Approved = (int)OrderStatus.Approved,
    Processing,
    WaitingToShip,
    Shipping,
    Complete,
    Cancelled,
}
