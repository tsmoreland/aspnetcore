using MicroShop.Services.Coupons.CouponApiApp.Infrastructure.Data;
using MicroShop.Services.Coupons.CouponApiApp.Models.DataTransferObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Coupons.CouponApiApp.Api;

internal static class CouponApiRouteGroupBuilderExtensions
{
    public static RouteGroupBuilder MapCouponsApi(this RouteGroupBuilder builder)
    {
        return builder
            .MapGetAllCoupons()
            .MapCouponGetById()
            .WithTags("Coupons");
    }

    private static RouteGroupBuilder MapGetAllCoupons(this RouteGroupBuilder builder)
    {
        builder
            .MapGet("/", ([FromServices] AppDbContext dbContext) =>
                Results.Ok(dbContext.Coupons.AsNoTracking()
                    .Select(c => new CouponDto(c))
                    .AsAsyncEnumerable()))
            .WithName("GetAllCoupons")
            .WithOpenApi();
        return builder;
    }

    private static RouteGroupBuilder MapCouponGetById(this RouteGroupBuilder builder)
    {
        builder
            .MapGet("/{id:int}", Handler)
            .WithName("GetCouponById")
            .WithOpenApi();
        return builder;

        async Task<Results<Ok<ResponseDto<CouponDto>>, NotFound<ResponseDto<CouponDto>>>> Handler([FromServices] AppDbContext dbContext, [FromRoute] int id)
        {
            CouponDto? dto = await dbContext.Coupons.AsNoTracking()
                .Select(c => new CouponDto(c))
                .FirstOrDefaultAsync(e => e.Id == id);
            return dto is not null
                ? TypedResults.Ok(new ResponseDto<CouponDto>(dto))
                : TypedResults.NotFound(new ResponseDto<CouponDto>(null, false, "Coupon matching id not found"));
        }
    }
}
