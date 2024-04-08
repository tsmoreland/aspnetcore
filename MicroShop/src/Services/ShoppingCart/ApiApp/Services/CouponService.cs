using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects;
using MicroShop.Services.ShoppingCart.ApiApp.Services.Contracts;

namespace MicroShop.Services.ShoppingCart.ApiApp.Services;

public sealed class CouponService(IHttpClientFactory clientFactory, ITokenProvider tokenProvider, ILogger<CouponService> logger) : ICouponService
{

    /// <inheritdoc />
    public async Task<CouponDto?> GetCouponByCode(string couponCode, CancellationToken cancellationToken = default)
    {
        if (couponCode is not { Length: > 0 })
        {
            logger.LogError("Coupon code cannot be empty");
            return null;
        }

        try
        {
            HttpResponseMessage responseMessage = await CreateClient().GetAsync($"/api/coupons/codes/{couponCode}", cancellationToken);
            if (!responseMessage.IsSuccessStatusCode)
            {
                logger.LogError("Get coupon by code failed with {StatusCode}", responseMessage.StatusCode);
                return null;
            }

            Func<ResponseDto<CouponDto>?, bool>[] validators = [GetCouponResponseIsNotNull, GetCouponResponseResponseIsSuccessful, GetCouponResponseDataIsNotNull];
            ResponseDto<CouponDto>? couponResponse = await responseMessage.Content.ReadFromJsonAsync<ResponseDto<CouponDto>>(cancellationToken);

            return validators.All(v => v(couponResponse))
                ? couponResponse!.Data
                : null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred");
            return null;
        }
    }

    private static bool GetCouponResponseIsNotNull(ResponseDto<CouponDto>? couponResponse) =>
        couponResponse is not null;
    private bool GetCouponResponseResponseIsSuccessful(ResponseDto<CouponDto>? couponResponse)
    {
        bool success = couponResponse?.Success is true;
        if (success)
        {
            return true;
        }

        logger.LogError("Coupon Response unsuccessful: {ErrorMessage}", couponResponse?.ErrorMessage ?? "Unknown Error");
        return false;
    }
    private bool GetCouponResponseDataIsNotNull(ResponseDto<CouponDto>? couponResponse)
    {
        if (couponResponse?.Data is not null)
        {
            return true;
        }

        logger.LogError("Get Coupon Response contained invalid content");
        return false;
    }


    private HttpClient CreateClient()
    {
        HttpClient client = clientFactory.CreateClient("CouponsApi");
        // could maybe make this a lazy variable, but not sure it's called enough for that to be of much benefit
        string? authorizationHeaderValue = tokenProvider.GetBearerToken();
        if (authorizationHeaderValue is { Length: > 0 })
        {
            client.DefaultRequestHeaders.Add("Authorization", authorizationHeaderValue);
        }
        return client;
    }
}
