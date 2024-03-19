using MicroShop.Web.App.Models;

namespace MicroShop.Web.App.Services.Contracts;

public interface IBaseService
{
    Task<ResponseDto?> SendAsync(string clientName, RequestDto request);
    Task<ResponseDto?> SendAsync<T>(string clientName, RequestDto<T> request);
}

