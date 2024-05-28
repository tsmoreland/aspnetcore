namespace MicroShop.Web.MvcApp.Models;

public record class RequestDto<T>(ApiType ApiType, string Url, T? Data)
{
    public RequestDto(string Url, T? Data)
        : this(ApiType.Get, Url, Data)
    {
    }
}


public record class RequestDto(ApiType ApiType, string Url, object? Data) : RequestDto<object>(ApiType, Url, Data)
{
    public RequestDto(string url)
        : this(ApiType.Get, url, null)
    {
    }

    public RequestDto(string url, object? data)
        : this(ApiType.Get, url, data)
    {
    }
}
