﻿namespace MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects.Response;

public sealed record CartSummaryDto(
    int Id,
    string? CouponCode,
    double Discount,
    double CartTotal,
    IEnumerable<CartItemDto> Details);
