using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Models.Coupons;
using MicroShop.Web.MvcApp.Services.Contracts;

namespace MicroShop.Web.MvcApp.Services;

public sealed class CouponService(IBaseService baseService) : ICouponService
{
    private readonly IBaseService _baseService = baseService ?? throw new ArgumentNullException(nameof(baseService));
    private const string ClientName = "CouponsApi";

    /// <inheritdoc />
    public async Task<ResponseDto<CouponDto>?> GetCouponById(int id)
    {
        return await SendAsync<CouponDto>(new RequestDto($"/api/coupons/{id}", null));
    }

    /// <inheritdoc />
    public async Task<ResponseDto<CouponDto>?> GetCouponByCode(string code)
    {
        return await SendAsync<CouponDto>(new RequestDto($"/api/coupons/code/{code}", null));
    }

    /// <inheritdoc />
    public async Task<ResponseDto<IEnumerable<CouponDto>>?> GetCoupons()
    {
        return await SendAsync<IEnumerable<CouponDto>>(new RequestDto("/api/coupons", null));
    }

    /// <inheritdoc />
    public async Task<ResponseDto<CouponDto>?> AddCoupon(AddOrEditCouponDto data)
    {
        return await SendAsync<CouponDto>(new RequestDto(ApiType.Post, "/api/coupons", data));
    }

    /// <inheritdoc />
    public async Task<ResponseDto<CouponDto>?> EditCoupon(int id, AddOrEditCouponDto data)
    {
        return await SendAsync<CouponDto>(new RequestDto(ApiType.Put, $"/api/coupons/{id}", data));
    }

    /// <inheritdoc />
    public async Task<ResponseDto<CouponDto>?> DeleteCoupon(int id)
    {
        return await SendAsync<CouponDto>(new RequestDto(ApiType.Delete, $"/api/coupons/{id}", null));
    }

    private Task<ResponseDto<T>?> SendAsync<T>(RequestDto data)
    {
        return _baseService.SendAsync<T>(ClientName, data);
    }
}
