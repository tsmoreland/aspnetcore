﻿using MicroShop.Services.ShoppingCart.App.Models.DataTransferObjects;
using MicroShop.Services.ShoppingCart.App.Models.DataTransferObjects.Request;
using MicroShop.Services.ShoppingCart.App.Models.DataTransferObjects.Response;

namespace MicroShop.Services.ShoppingCart.App.Services.Contracts;

public interface ICartService
{
    Task<ResponseDto<CartSummaryDto>> Upsert(string userId, UpsertCartDto item, CancellationToken cancellationToken = default);
    Task<ResponseDto> RemoveFromCart(string userId, int cartDetailsId, CancellationToken cancellationToken = default);
    Task<ResponseDto<CartSummaryDto>> GetByUserId(string userId, CancellationToken cancellationToken = default);
    Task<ResponseDto> EmailCart(string userId, string name, string emailAddress, CancellationToken cancellationToken = default);
    Task<ResponseDto> ApplyCoupon(string userId, int cartHeaderId, string? couponCode, CancellationToken cancellationToken = default);
    Task<ResponseDto> RemoveCoupon(string userId, int cartHeaderId, CancellationToken cancellationToken = default);

}
