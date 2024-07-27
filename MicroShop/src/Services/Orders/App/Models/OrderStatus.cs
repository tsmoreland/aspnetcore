namespace MicroShop.Services.Orders.App.Models;

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
