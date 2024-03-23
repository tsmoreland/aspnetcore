namespace MicroShop.Services.Auth.AuthApiApp.Models.DataTransferObjects;

public sealed record class ResponseDto(bool Success = true, string? ErrorMessage = null) 
{
    public static ResponseDto<T> Ok<T>(T value)
    {
        return new ResponseDto<T>(value, true, null);
    }
    public static ResponseDto Ok()
    {
        return new ResponseDto(true, null);
    }
    public static ResponseDto<T> Error<T>(string errorMessage)
    {
        return new ResponseDto<T>(default, false, errorMessage);
    }
    public static ResponseDto Error(string errorMessage)
    {
        return new ResponseDto(false, errorMessage);
    }

}

public record class ResponseDto<T>(T? Data, bool Success = true, string? ErrorMessage = null);
