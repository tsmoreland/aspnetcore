﻿using System.Net;
using MicroShop.Web.Mvc.App.Models;
using MicroShop.Web.Mvc.App.Models.Coupons;
using MicroShop.Web.Mvc.App.Services;

namespace MicroShop.Web.Mvc.App.Tests.Services;

public sealed class CouponServiceTest
{
    private readonly TestHttpClientFactory _factory = new();
    private readonly TestTokenProvider _tokenProvider = new();
    private const string ClientName = "CouponsApi";

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenBaseServiceIsNull()
    {
        ArgumentNullException? ex = Assert.Throws<ArgumentNullException>(() => _ = new CouponService(null!));
        ex.ParamName.Should().Be("baseService");
    }

    [Fact]
    public async Task GetCouponById_ShouldSuccessfulResponse_WhenApiReturnsSuccessfulResponse()
    {
        TestDelegatingHandler handler = _factory.AddHandler(ClientName);
        ResponseDto<CouponDto> expected = new(new CouponDto(1, "10off", 10.0, 5));
        handler.AddResponse(new MethodUrlKey(HttpMethod.Get, new Uri($"{TestHttpClientFactory.BaseAddress}/api/coupons/1")), new TestResponse(HttpStatusCode.OK, expected));
        BaseService baseService = new(_factory, _tokenProvider);
        CouponService couponService = new(baseService);

        ResponseDto<CouponDto>? actual = await couponService.GetCouponById(1);

        actual.Should().Be(expected);
    }
}
