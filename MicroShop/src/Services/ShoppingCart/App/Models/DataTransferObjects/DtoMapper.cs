﻿using MicroShop.Services.ShoppingCart.App.Models.DataTransferObjects.Response;

namespace MicroShop.Services.ShoppingCart.App.Models.DataTransferObjects;

internal static class DtoMapper
{
    public static CartSummaryDto ToCartSummary(this CartDto cartDto)
    {
        ArgumentNullException.ThrowIfNull(cartDto);

        return new CartSummaryDto(
            cartDto.Header.Id,
            cartDto.Header.CouponCode,
            cartDto.Header.Discount,
            cartDto.Header.CartTotal,
            cartDto.Details
                .Select(static item => item.ToCartItem())
                .ToList());
    }

    public static CartItemDto ToCartItem(this CartDetailsDto details) =>
        new(details.Id, details.Product.Id, details.Product.Name, details.Product.Description, details.Product.Price, details.Product.ImageUrl, details.Count);

    public static CartItemDto ToCartItem(this CartDetails details) =>
        new(details.Id, details.Product.Id, details.Product.Name, details.Product.Description, details.Product.Price, details.Product.ImageUrl, details.Count);
}
