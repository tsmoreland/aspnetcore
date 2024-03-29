using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Models.Coupons;
using MicroShop.Web.MvcApp.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MicroShop.Web.MvcApp.Controllers;

public sealed class CouponController(ICouponService couponService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ResponseDto<IEnumerable<CouponDto>>? response = await couponService.GetCoupons();
        if (response?.Success is not true || response?.Data is null)
        {
            TempData["error"] = response?.ErrorMessage;
            return View(new List<CouponDto>());
        }

        List<CouponDto> coupons = response.Data.ToList();
        return View(coupons);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new AddOrEditCouponDto("", 0.0, 0));
    }

    [HttpPost]
    public async Task<IActionResult> Create(AddOrEditCouponDto coupon)
    {
        if (!ModelState.IsValid)
        {
            return View(coupon);
        }

        ResponseDto<CouponDto>? response = await couponService.AddCoupon(coupon);
        if (response?.Success is true)
        {
            TempData["success"] = "Coupon Created Successfully";
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData["error"] = response?.ErrorMessage;
        }

        return View(coupon);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int couponId)
    {
        ResponseDto<CouponDto>? response = await couponService.GetCouponById(couponId);
        if (response is { Success: true, Data: not null })
        {
            return View(response.Data);
        }
        else
        {
            TempData["error"] = response?.ErrorMessage;
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(CouponDto coupon)
    {
        ResponseDto<CouponDto>? response = await couponService.DeleteCoupon(coupon.Id);
        if (response?.Success is true)
        {
            TempData["success"] = "Coupon deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData["error"] = response?.ErrorMessage;
        }
        return View(coupon);
    }
}
