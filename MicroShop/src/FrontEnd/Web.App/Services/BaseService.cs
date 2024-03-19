using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using MicroShop.Web.App.Models;
using MicroShop.Web.App.Services.Contracts;
using Microsoft.AspNetCore.Http.Features;

namespace MicroShop.Web.App.Services;

public sealed class BaseService(IHttpClientFactory clientFactory) : IBaseService 
{
    private readonly IHttpClientFactory _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));

    /// <inheritdoc />
    public async Task<ResponseDto?> SendAsync(string clientName, RequestDto request)
    {
        (HttpClient client, HttpRequestMessage message) = SetupRequest(clientName, request.ApiType, request.Url);

        if (request.Data is not null)
        {
            // not very efficient, but it'll do for now
            message.Content = new StringContent(JsonSerializer.Serialize(request.Data), Encoding.UTF8, MediaTypeNames.Application.Json);
        }
        return await SendRequestAsync(client, message);
    }

    /// <inheritdoc />
    public async Task<ResponseDto?> SendAsync<T>(string clientName, RequestDto<T> request)
    {
        (HttpClient client, HttpRequestMessage message) = SetupRequest(clientName, request.ApiType, request.Url);

        if (request.Data is not null)
        {
            // not very efficient, but it'll do for now
            message.Content = new StringContent(JsonSerializer.Serialize(request.Data), Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        return await SendRequestAsync(client, message);
    }

    private (HttpClient Client, HttpRequestMessage Message) SetupRequest(string clientName, ApiType apiType, string url)
    {
        HttpClient client = _clientFactory.CreateClient(clientName);
        HttpRequestMessage message = new();
        // token (pending)

        message.RequestUri = new Uri(url);
        message.Method = apiType switch
        {
            ApiType.Post => HttpMethod.Post,
            ApiType.Put => HttpMethod.Put,
            ApiType.Delete => HttpMethod.Delete,
            _ => HttpMethod.Get,
        };

        return (client, message);
    }

    private static async Task<ResponseDto?> SendRequestAsync(HttpClient client, HttpRequestMessage message)
    {
        HttpResponseMessage? response = await client.SendAsync(message);
        try
        {
            return await response.Content.ReadFromJsonAsync<ResponseDto>();
        }
        catch (Exception)
        {
            return response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => new ResponseDto(null, false, "Forbidden"),
                HttpStatusCode.Forbidden => new ResponseDto(null, false, "Not authorized"),
                HttpStatusCode.NotFound => new ResponseDto(null, false, "Not Found"),
                _ => new ResponseDto(null, false, "Internal Server Error"),
            };
        }
    }

}
