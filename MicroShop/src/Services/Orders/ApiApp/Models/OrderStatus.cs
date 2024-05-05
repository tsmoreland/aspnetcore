namespace MicroShop.Services.Orders.ApiApp.Models;

public enum OrderStatus
{
    Pending,
    Approved,
    Processing,
    WaitingToShip,
    Shipping,
    Complete,
}
