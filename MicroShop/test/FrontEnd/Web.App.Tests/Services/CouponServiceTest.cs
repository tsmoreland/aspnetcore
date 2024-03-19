using MicroShop.Web.App.Services;

namespace MicroShop.Web.App.Tests.Services;

public sealed class CouponServiceTest
{
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenBaseServiceIsNull()
    {
        ArgumentNullException? ex = Assert.Throws<ArgumentNullException>(() => _ = new CouponService(null!));
        ex.ParamName.Should().Be("baseService");
    }
}
