using MicroShop.Web.MvcApp.Models;

namespace MicroShop.Web.MvcApp.Services.Contracts;

public interface IBaseService
{
    Task<ResponseDto<TResponse>?> SendAsync<TResponse>(string clientName, RequestDto request, SupportedContentType supportedContentType = SupportedContentType.ApplicationJson, bool includeAuthorization = true);
    Task<ResponseDto<TResponse>?> SendAsync<TRequest, TResponse>(string clientName, RequestDto<TRequest> request, SupportedContentType supportedContentType = SupportedContentType.ApplicationJson, bool includeAuthorization = true);

    Task<ResponseDto?> SendAsync(string clientName, RequestDto request, SupportedContentType supportedContentType = SupportedContentType.ApplicationJson, bool includeAuthorization = true);
}
