using MicroShop.Web.App.Models;

namespace MicroShop.Web.App.Services.Contracts;

public interface IBaseService
{
    Task<ResponseDto<TResponse>?> SendAsync<TResponse>(string clientName, RequestDto request);
    Task<ResponseDto<TResponse>?> SendAsync<TRequest, TResponse>(string clientName, RequestDto<TRequest> request);

    Task<ResponseDto?> SendAsync(string clientName, RequestDto request);
}

