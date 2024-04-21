namespace MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Responses;

public sealed record OrderSummaryDto(
    int Id,
    OrderStatus Status,
    string? CouponCode,
    double Discount,
    double OrderTotal,
    IEnumerable<OrderItemDto> Details);
