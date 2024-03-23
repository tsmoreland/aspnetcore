namespace MicroShop.Web.MvcApp.Models;

public record class RequestDto<T>(ApiType ApiType, string Url, T? Data, string AccessToken)
{
    public RequestDto(string Url, T? Data, string AccessToken)
        : this(ApiType.Get, Url, Data, AccessToken)
    {
    }
}


public sealed record class RequestDto(ApiType ApiType, string Url, object? Data, string AccessToken) : RequestDto<object>(ApiType, Url, Data, AccessToken)
{
    public RequestDto(string Url, object? Data, string AccessToken)
        : this(ApiType.Get, Url, Data, AccessToken)
    {
    }
}
