using MicroShop.Web.MvcApp.Models;

namespace MicroShop.Web.MvcApp.Services.Contracts;

public interface IBaseService
{
    Task<ResponseDto<TResponse>?> SendAsync<TResponse>(string clientName, RequestDto request, bool includeAuthorization = true);
    Task<ResponseDto<TResponse>?> SendAsync<TRequest, TResponse>(string clientName, RequestDto<TRequest> request, bool includeAuthorization = true);

    Task<ResponseDto?> SendAsync(string clientName, RequestDto request, bool includeAuthorization = true);
}

