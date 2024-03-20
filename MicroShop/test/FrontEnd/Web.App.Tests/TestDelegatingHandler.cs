using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using MicroShop.Web.App.Models;
using MicroShop.Web.App.Models.Coupons;

namespace MicroShop.Web.App.Tests;

public sealed record class TestResponse(HttpStatusCode StatusCode, object? Content, string ContentType = "application/json");

public sealed class TestDelegatingHandler : DelegatingHandler
{
    private readonly Dictionary<MethodUrlKey, TestResponse> _responsesByMethodUrl = [];

    public void AddResponse(MethodUrlKey key, TestResponse response)
    {
        _responsesByMethodUrl[key] = response;
    }

    public void ClearResponses()
    {
        _responsesByMethodUrl.Clear();
    }


    /// <inheritdoc />
    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        MethodUrlKey key = new(request.Method, request.RequestUri);
        HttpResponseMessage response = new();

        if (!_responsesByMethodUrl.TryGetValue(key, out TestResponse? responseData))
        {
            return response;
        }

        response.StatusCode = responseData.StatusCode;

        string jsonData = responseData.Content is not null
            ? JsonSerializer.Serialize(responseData.Content)
            : JsonSerializer.Serialize(new ResponseDto<object>(null, false, "-- response not present in dictionary --"));

        response.Content = new StringContent(jsonData, Encoding.UTF8, responseData.ContentType);
        return response;
    }

    /// <inheritdoc />
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Send(request, cancellationToken));
    }
}

