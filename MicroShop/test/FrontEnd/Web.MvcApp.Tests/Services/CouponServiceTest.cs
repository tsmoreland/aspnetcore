using System.Net;
using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Models.Coupons;
using MicroShop.Web.MvcApp.Services;

namespace MicroShop.Web.MvcApp.Tests.Services;

public sealed class CouponServiceTest
{
    private readonly TestHttpClientFactory _factory = new();
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
        BaseService baseService = new(_factory);
        CouponService couponService = new(baseService);

        ResponseDto<CouponDto>? actual = await couponService.GetCouponById(1);

        actual.Should().Be(expected);
    }
}
