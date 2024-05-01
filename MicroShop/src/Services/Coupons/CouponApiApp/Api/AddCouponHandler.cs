using System.Globalization;
using MicroShop.Services.Coupons.CouponApiApp.Infrastructure.Data;
using MicroShop.Services.Coupons.CouponApiApp.Models;
using MicroShop.Services.Coupons.CouponApiApp.Models.DataTransferObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using CouponResponse = MicroShop.Services.Coupons.CouponApiApp.Models.DataTransferObjects.ResponseDto<MicroShop.Services.Coupons.CouponApiApp.Models.DataTransferObjects.CouponDto>;

namespace MicroShop.Services.Coupons.CouponApiApp.Api;

public sealed class AddCouponHandler(AppDbContext dbContext, ILogger<AddCouponHandler> logger)
{
    public async Task<Results<Created<CouponResponse>, BadRequest<CouponResponse>>> Handle(AddOrEditCouponDto model)
    {
        try
        {
            Coupon coupon = model.ToNewCoupon();
            dbContext.Coupons.Add(coupon);
            await dbContext.SaveChangesAsync();

            Stripe.CouponCreateOptions options = new()
            {
                AmountOff = (long)model.DiscountAmount * 100, // converting from $ or GBP to cents or pence
                Name = model.Code,
                Id = coupon.Id.ToString(CultureInfo.InvariantCulture),
                Currency = "gbp", // should come from config or definitions class
                Duration = "repeating", // again from dto via enum and some mapper that maps enum to string
                DurationInMonths = 3, // and once again - use the dto
            }; 
            Stripe.CouponService couponService = new();
            await couponService.CreateAsync(options);

            CouponDto result = new(coupon);
            return TypedResults.Created((string?)null, ResponseDto.Ok(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to add coupon");
            // cheap out and blame the client for now, more precise exception handling would handle this better
            return TypedResults.BadRequest(ResponseDto.Error<CouponDto>("One or more properties of the provided data are invalid"));
        }
    }
}
