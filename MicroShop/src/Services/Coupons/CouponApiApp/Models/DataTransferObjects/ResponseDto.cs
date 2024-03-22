namespace MicroShop.Services.Coupons.CouponApiApp.Models.DataTransferObjects;

public sealed record class ResponseDto(object? Data, bool Success = true, string? ErrorMessage = null) : ResponseDto<object>(Data, Success, ErrorMessage)
{
    public static ResponseDto<T> Ok<T>(T value)
    {
        return new ResponseDto<T>(value, true, null);
    }
    public static ResponseDto<T> Error<T>(string errorMessage)
    {
        return new ResponseDto<T>(default, false, errorMessage);
    }
}
public record class ResponseDto<T>(T? Data, bool Success = true, string? ErrorMessage = null);
