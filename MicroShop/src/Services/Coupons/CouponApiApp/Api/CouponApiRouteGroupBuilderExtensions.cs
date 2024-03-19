using MicroShop.Services.Coupons.CouponApiApp.Infrastructure.Data;
using MicroShop.Services.Coupons.CouponApiApp.Models;
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
            .MapPost()
            .MapGetAllCoupons()
            .MapGetCouponById()
            .MapGetCouponByCode()
            .MapPut()
            .MapDelete()
            .WithTags("Coupons");
    }

    private static RouteGroupBuilder MapPost(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("/", async ([FromBody] AddOrEditCouponDto data, [FromServices] AppDbContext dbContext) =>
            {
                try
                {
                    Coupon coupon = data.ToNewCoupon();
                    dbContext.Coupons.Add(coupon);
                    await dbContext.SaveChangesAsync();

                    CouponDto result = new(coupon);
                    return Results.Created((string?)null, new ResponseDto(result));
                }
                catch (Exception ex)
                {
                    // cheap out and blame the client for now, more precise exception handling would handle this better
                    return Results.BadRequest(new ResponseDto<CouponDto>(null, false, "One or more properties of the provided data are invalid"));
                }
            })
            .WithName("AddCoupon")
            .WithOpenApi();
        return builder;
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

    private static RouteGroupBuilder MapGetCouponById(this RouteGroupBuilder builder)
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

    private static RouteGroupBuilder MapGetCouponByCode(this RouteGroupBuilder builder)
    {
        builder
            .MapGet("/codes/{code}", Handler)
            .WithName("GetCouponByCode")
            .WithOpenApi();
        return builder;

        async Task<Results<Ok<ResponseDto<CouponDto>>, NotFound<ResponseDto<CouponDto>>>> Handler([FromServices] AppDbContext dbContext, [FromRoute] string code)
        {
            string normalizedCode = (code ?? string.Empty).ToUpperInvariant();
            CouponDto? dto = await dbContext.Coupons.AsNoTracking()
                .Where(c => c.NormalizedCode == normalizedCode)
                .Select(c => new CouponDto(c))
                .FirstOrDefaultAsync();
            return dto is not null
                ? TypedResults.Ok(new ResponseDto<CouponDto>(dto))
                : TypedResults.NotFound(new ResponseDto<CouponDto>(null, false, "Coupon matching id not found"));
        }
    }

    private static RouteGroupBuilder MapPut(this RouteGroupBuilder builder)
    {
        builder
            .MapPut("/{id:int}", async ([FromRoute] int id, [FromBody] AddOrEditCouponDto data, [FromServices] AppDbContext dbContext) =>
            {
                try
                {
                    Coupon coupon = data.ToExistingCoupon(id);
                    dbContext.Coupons.Update(coupon);
                    await dbContext.SaveChangesAsync();

                    CouponDto result = new(coupon);
                    return Results.Ok(new ResponseDto(result));

                }
                catch (Exception ex)
                {
                    // cheap out and blame the client for now, more precise exception handling would handle this better
                    return Results.BadRequest(new ResponseDto<CouponDto>(null, false, "One or more properties of the provided data are invalid"));
                }
            })
            .WithName("UpdateCoupon")
            .WithOpenApi();
        return builder;
    }

    private static RouteGroupBuilder MapDelete(this RouteGroupBuilder builder)
    {
        builder
            .MapDelete("/{id:int}", async ([FromRoute] int id, [FromServices] AppDbContext dbContext) =>
            {
                try
                {
                    int deleted = await dbContext.Coupons.Where(c => c.Id == id).ExecuteDeleteAsync();
                    _ = deleted; // may want to log this
                    return NoContentWithResponseResult.Success();
                }
                catch (Exception ex)
                {
                    // cheap out and blame the client for now, more precise exception handling would handle this better
                    return Results.BadRequest(new ResponseDto<CouponDto>(null, false, "One or more properties of the provided data are invalid"));
                }
            })
            .WithName("DeleteCoupon")
            .WithOpenApi();

        return builder;
    }

}
