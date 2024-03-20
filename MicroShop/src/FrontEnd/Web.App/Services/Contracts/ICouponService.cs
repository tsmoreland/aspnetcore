using MicroShop.Web.App.Models;
using MicroShop.Web.App.Models.Coupons;

namespace MicroShop.Web.App.Services.Contracts;

public interface ICouponService
{
    public Task<ResponseDto<CouponDto>?> GetCouponById(int id);
    public Task<ResponseDto<CouponDto>?> GetCouponByCode(string code);
    public Task<ResponseDto<IEnumerable<CouponDto>>?> GetCoupons(int id);
    public Task<ResponseDto<CouponDto>?> AddCoupon(AddOrEditCouponDto data);
    public Task<ResponseDto<CouponDto>?> EditCoupon(int id, AddOrEditCouponDto data);
    public Task<ResponseDto<CouponDto>?> DeleteCoupon(int id);

}
