namespace MicroShop.Services.Orders.App.Models.DataTransferObjects.Requests;

public enum OrderUpdateStatus
{
    Approved = (int)OrderStatus.Approved,
    Processing,
    WaitingToShip,
    Shipping,
    Complete,
    Cancelled,
}
