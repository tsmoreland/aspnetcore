namespace MicroShop.Web.App.Models;

public sealed record class ResponseDto(object? Data, bool Success = true, string? ErrorMessage = null) : ResponseDto<object>(Data, Success, ErrorMessage)
{
    public ResponseDto<T> ToTypedResponseOrThrow<T>()
    {
        return Data switch
        {
            null => new ResponseDto<T>(default, Success, ErrorMessage),
            T data => new ResponseDto<T>(data, Success, ErrorMessage),
            _ => throw new InvalidCastException($"Unable to convert to RespponseDto<T> where T is `{typeof(T).Name}`")
        };
    }

}
public record class ResponseDto<T>(T? Data, bool Success = true, string? ErrorMessage = null);
