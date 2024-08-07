﻿using System.Globalization;
using MicroShop.Services.Coupons.App.Infrastructure.Data;
using MicroShop.Services.Coupons.App.Models.DataTransferObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Coupon = MicroShop.Services.Coupons.App.Models.Coupon;

namespace MicroShop.Services.Coupons.App.Api;

internal static class CouponApiRouteGroupBuilderExtensions
{
    public static RouteGroupBuilder MapCouponsApi(this RouteGroupBuilder builder)
    {
        return builder
            .MapAddCoupon()
            .MapGetAllCoupons()
            .MapGetCouponById()
            .MapGetCouponByCode()
            .MapPut()
            .MapDelete()
            .WithTags("Coupons");
    }

    private static RouteGroupBuilder MapAddCoupon(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("/", static ([FromBody] AddOrEditCouponDto data, [FromServices] AddCouponHandler handler) => handler.Handle(data))
            .RequireAuthorization("ADMIN")
            .WithName("AddCoupon")
            .WithOpenApi();
        return builder;
    }

    private static RouteGroupBuilder MapGetAllCoupons(this RouteGroupBuilder builder)
    {
        builder
            .MapGet("/", async ([FromServices] AppDbContext dbContext) =>
                Results.Ok(new ResponseDto<IEnumerable<CouponDto>>(await dbContext.Coupons.AsNoTracking()
                    .Select(c => new CouponDto(c))
                    .ToListAsync())))
            .RequireAuthorization()
            .WithName("GetAllCoupons")
            .WithOpenApi();
        return builder;
    }

    private static RouteGroupBuilder MapGetCouponById(this RouteGroupBuilder builder)
    {
        builder
            .MapGet("/{id:int}", Handler)
            .RequireAuthorization()
            .WithName("GetCouponById")
            .WithOpenApi();
        return builder;

        async Task<Results<Ok<ResponseDto<CouponDto>>, NotFound<ResponseDto<CouponDto>>>> Handler([FromServices] AppDbContext dbContext, [FromRoute] int id)
        {
            CouponDto? dto = await dbContext.Coupons.AsNoTracking()
                .Where(e => e.Id == id)
                .Select(c => new CouponDto(c))
                .AsAsyncEnumerable()
                .FirstOrDefaultAsync();
            return dto is not null
                ? TypedResults.Ok(ResponseDto.Ok(dto))
                : TypedResults.NotFound(ResponseDto.Error<CouponDto>("Coupon matching id not found"));
        }
    }

    private static RouteGroupBuilder MapGetCouponByCode(this RouteGroupBuilder builder)
    {
        builder
            .MapGet("/codes/{code}", Handler)
            .RequireAuthorization()
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
                ? TypedResults.Ok(ResponseDto.Ok(dto))
                : TypedResults.NotFound(ResponseDto.Error<CouponDto>("Coupon matching id not found"));
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
                    return Results.Ok(ResponseDto.Ok(result));

                }
                catch (Exception)
                {
                    // cheap out and blame the client for now, more precise exception handling would handle this better
                    return Results.BadRequest(ResponseDto.Error<CouponDto>("One or more properties of the provided data are invalid"));
                }
            })
            .RequireAuthorization("ADMIN")
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

                    CouponService couponService = new();
                    await couponService.DeleteAsync(id.ToString(CultureInfo.InvariantCulture));

                    return NoContentWithResponseResult.Success();
                }
                catch (Exception)
                {
                    // cheap out and blame the client for now, more precise exception handling would handle this better
                    return Results.BadRequest(ResponseDto.Error<CouponDto>("One or more properties of the provided data are invalid"));
                }
            })
            .RequireAuthorization("ADMIN")
            .WithName("DeleteCoupon")
            .WithOpenApi();

        return builder;
    }

}
