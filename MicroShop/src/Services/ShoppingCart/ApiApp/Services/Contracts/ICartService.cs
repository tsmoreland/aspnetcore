using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects;
using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects.Request;
using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects.Response;

namespace MicroShop.Services.ShoppingCart.ApiApp.Services.Contracts;

public interface ICartService
{
    Task<ResponseDto<CartSummaryDto>> Upsert(string userId, UpsertCartDto item, CancellationToken cancellationToken = default);
}
