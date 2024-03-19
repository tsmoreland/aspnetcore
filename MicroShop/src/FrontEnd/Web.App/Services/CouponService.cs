using MicroShop.Web.App.Models;
using MicroShop.Web.App.Models.Coupons;
using MicroShop.Web.App.Services.Contracts;

namespace MicroShop.Web.App.Services;

public sealed class CouponService(IBaseService baseService) : ICouponService
{
    private readonly IBaseService _baseService = baseService ?? throw new ArgumentNullException(nameof(baseService));
    private const string ClientName = "CouponsApi";

    /// <inheritdoc />
    public async Task<ResponseDto<CouponDto>?> GetCouponById(int id)
    {
        ResponseDto? response = await SendAsync(new RequestDto($"/api/coupons/{id}", null, string.Empty));
        return response?.ToTypedResponseOrThrow<CouponDto>();
    }

    /// <inheritdoc />
    public async Task<ResponseDto<CouponDto>?> GetCouponByCode(string code)
    {
        ResponseDto? response = await SendAsync(new RequestDto($"/api/coupons/code/{code}", null, string.Empty));
        return response?.ToTypedResponseOrThrow<CouponDto>();
    }

    /// <inheritdoc />
    public async Task<ResponseDto<IEnumerable<CouponDto>>?> GetCoupons(int id)
    {
        ResponseDto? response = await SendAsync(new RequestDto("/api/coupons", null, string.Empty));

        // this will fail at present because it's actually an IAsyncEnumerable
        return response?.ToTypedResponseOrThrow<IEnumerable<CouponDto>>();
    }

    /// <inheritdoc />
    public async Task<ResponseDto<CouponDto>?> AddCoupon(AddOrEditCouponDto data)
    {
        ResponseDto? response = await SendAsync(new RequestDto(ApiType.Post, "/api/coupons", data, string.Empty));
        return response?.ToTypedResponseOrThrow<CouponDto>();
    }

    /// <inheritdoc />
    public async Task<ResponseDto<CouponDto>?> EditCoupon(int id, AddOrEditCouponDto data)
    {
        ResponseDto? response = await SendAsync(new RequestDto(ApiType.Put, $"/api/coupons/{id}", data, string.Empty));
        return response?.ToTypedResponseOrThrow<CouponDto>();
    }

    /// <inheritdoc />
    public async Task<ResponseDto?> DeleteCoupon(int id)
    {
        return await SendAsync(new RequestDto(ApiType.Delete, $"/api/coupons/{id}", null, string.Empty));
    }

    private Task<ResponseDto?> SendAsync(RequestDto data) =>
        _baseService.SendAsync(ClientName, data);
}
