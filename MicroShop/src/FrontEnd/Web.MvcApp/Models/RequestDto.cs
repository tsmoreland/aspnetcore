namespace MicroShop.Web.MvcApp.Models;

public sealed record class RequestDto<T>(ApiType ApiType, string Url, T? Data) : RequestDto(ApiType, Url)
{
    public RequestDto(string Url, T? Data)
        : this(ApiType.Get, Url, Data)
    {
    }
}


public record class RequestDto(ApiType ApiType, string Url) 
{
    public RequestDto(string Url)
        : this(ApiType.Get, Url)
    {
    }
}
