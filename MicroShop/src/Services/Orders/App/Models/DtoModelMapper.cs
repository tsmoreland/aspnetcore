using MicroShop.Services.Orders.App.Models.DataTransferObjects.Requests;
using MicroShop.Services.Orders.App.Models.DataTransferObjects.Responses;

namespace MicroShop.Services.Orders.App.Models;

internal static class DtoModelMapper
{
    public static OrderHeader ToOrderHeader(this CreateOrderDto dto, string userId)
    {
        return OrderHeader.CreateOrder(userId, dto.Cart.CouponCode, dto.Cart.Discount, dto.Cart.CartTotal, dto.Name, dto.EmailAddress, dto.Cart.Details, CreateOrderItem);

        static OrderDetails CreateOrderItem(OrderHeader header, CartItemDto itemDto)
        {
            return new OrderDetails(header, itemDto.ProductId, itemDto.ProductName, itemDto.Price, itemDto.Count);
        }
    }

    public static OrderSummaryDto ToSummary(this OrderHeader order)
    {
        return new OrderSummaryDto(order.Id, order.Status, order.CouponCode, order.Discount, order.OrderTotal, order.Items.Select(ToDto).ToHashSet());

        static OrderItemDto ToDto(OrderDetails details) =>
            new(details.Id, details.ProductId, details.ProductName, details.Price, details.Count);
    }
}
