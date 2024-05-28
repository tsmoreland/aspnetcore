namespace MicroShop.Services.Products.ProductsApiApp.Models.DataTransferObjects;

public record class ResponseDto(bool Success = true, string? ErrorMessage = null)
{
    public static ResponseDto<T> Ok<T>(T value) => new(value, true, null);
    public static ResponseDto Ok() => new(true, null);
    public static ResponseDto<T> Error<T>(string errorMessage) => new(default, false, errorMessage);
    public static ResponseDto Error(string errorMessage) => new(false, errorMessage);
}
public sealed record class ResponseDto<T>(T? Data, bool Success = true, string? ErrorMessage = null) : ResponseDto(Success, ErrorMessage);
