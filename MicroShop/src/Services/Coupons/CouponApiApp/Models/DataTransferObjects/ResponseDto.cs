namespace MicroShop.Services.Coupons.CouponApiApp.Models.DataTransferObjects;

public sealed record class ResponseDto(object? Data, bool Success = true, string? ErrorMessage = null) : ResponseDto<object>(Data, Success, ErrorMessage);
public record class ResponseDto<T>(T? Data, bool Success = true, string? ErrorMessage = null);
