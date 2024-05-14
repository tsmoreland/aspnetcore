namespace MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Requests;

public enum OrderUpdateStatus
{
    Approved = (int)OrderStatus.Approved,
    Processing,
    WaitingToShip,
    Shipping,
    Complete,
    Cancelled,
}
