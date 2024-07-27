using MicroShop.Web.Mvc.App.Models;
using MicroShop.Web.Mvc.App.Models.Coupons;

namespace MicroShop.Web.Mvc.App.Services.Contracts;

public interface ICouponService
{
    public Task<ResponseDto<CouponDto>?> GetCouponById(int id);
    public Task<ResponseDto<CouponDto>?> GetCouponByCode(string code);
    public Task<ResponseDto<IEnumerable<CouponDto>>?> GetCoupons();
    public Task<ResponseDto<CouponDto>?> AddCoupon(AddOrEditCouponDto data);
    public Task<ResponseDto<CouponDto>?> EditCoupon(int id, AddOrEditCouponDto data);
    public Task<ResponseDto<CouponDto>?> DeleteCoupon(int id);

}
