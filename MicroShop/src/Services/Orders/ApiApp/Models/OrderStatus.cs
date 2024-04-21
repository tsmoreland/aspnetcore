namespace MicroShop.Services.Orders.ApiApp.Models;

public enum OrderStatus
{
    Pending,
    Processing,
    WaitingToShip,
    Shipping,
    Complete,
}
