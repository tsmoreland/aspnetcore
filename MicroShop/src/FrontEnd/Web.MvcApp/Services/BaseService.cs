using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Services.Contracts;

namespace MicroShop.Web.MvcApp.Services;

public sealed class BaseService(IHttpClientFactory clientFactory, ITokenProvider tokenProvider) : IBaseService 
{
    private readonly IHttpClientFactory _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));

    /// <inheritdoc />
    public async Task<ResponseDto<TResponse>?> SendAsync<TResponse>(string clientName, RequestDto request, SupportedContentType contentType = SupportedContentType.ApplicationJson, bool includeAuthorization = true)
    {
        (HttpClient client, HttpRequestMessage message) = SetupRequest(clientName, request.ApiType, request.Url, includeAuthorization, contentType);

        message.Content = contentType.BuildHttpContent(request.Data);
        return await SendRequestAsync<TResponse>(client, message);
    }

    /// <inheritdoc />
    public async Task<ResponseDto<TResponse>?> SendAsync<TRequest, TResponse>(string clientName, RequestDto<TRequest> request, SupportedContentType contentType = SupportedContentType.ApplicationJson, bool includeAuthorization = true)
    {
        (HttpClient client, HttpRequestMessage message) = SetupRequest(clientName, request.ApiType, request.Url, includeAuthorization, contentType);
        message.Content = contentType.BuildHttpContent(request.Data);
        return await SendRequestAsync<TResponse>(client, message);
    }

    /// <inheritdoc />
    public Task<ResponseDto?> SendAsync(string clientName, RequestDto request, SupportedContentType contentType = SupportedContentType.ApplicationJson, bool includeAuthorization = true)
    {
        (HttpClient client, HttpRequestMessage message) = SetupRequest(clientName, request.ApiType, request.Url, includeAuthorization, contentType);
        message.Content = contentType.BuildHttpContent(request.Data);
        return SendRequestAsync(client, message);
    }

    private (HttpClient Client, HttpRequestMessage Message) SetupRequest(string clientName, ApiType apiType, string url, bool includeAuthorization, SupportedContentType contentType)
    {
        HttpClient client = _clientFactory.CreateClient(clientName);
        HttpRequestMessage message = new()
        {
            RequestUri = url.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? new Uri(url) : new Uri(url, UriKind.Relative),
            Method = apiType switch
            {
                ApiType.Post => HttpMethod.Post,
                ApiType.Put => HttpMethod.Put,
                ApiType.Delete => HttpMethod.Delete,
                _ => HttpMethod.Get,
            }
        };
        if (includeAuthorization && tokenProvider.GetToken() is {} token)
        {
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        message.Headers.Add("Accept", contentType.ToAcceptMediaTypeName());

        return (client, message);
    }

    private static async Task<ResponseDto<TResponse>?> SendRequestAsync<TResponse>(HttpClient client, HttpRequestMessage message)
    {
        HttpResponseMessage? response = await client.SendAsync(message);
        try
        {
            ResponseDto<TResponse>? responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TResponse>>();
            return responseDto ?? new ResponseDto<TResponse>(default, false, "Unable to deserialized json content");
        }
        catch (Exception)
        {
            return response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => new ResponseDto<TResponse>(default, false, "Forbidden"),
                HttpStatusCode.Forbidden => new ResponseDto<TResponse>(default, false, "Not authorized"),
                HttpStatusCode.NotFound => new ResponseDto<TResponse>(default, false, "Not Found"),
                _ => new ResponseDto<TResponse>(default, false, "Internal Server Error"),
            };
        }
    }
    private static async Task<ResponseDto?> SendRequestAsync(HttpClient client, HttpRequestMessage message)
    {
        HttpResponseMessage? response = await client.SendAsync(message);
        try
        {
            ResponseDto? responseDto = await response.Content.ReadFromJsonAsync<ResponseDto>();
            return responseDto ?? new ResponseDto(false, "Unable to deserialized json content");
        }
        catch (Exception)
        {
            return response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => new ResponseDto(false, "Forbidden"),
                HttpStatusCode.Forbidden => new ResponseDto(false, "Not authorized"),
                HttpStatusCode.NotFound => new ResponseDto(false, "Not Found"),
                _ => new ResponseDto(false, "Internal Server Error"),
            };
        }
    }

}
