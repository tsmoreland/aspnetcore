
namespace MicroShop.Web.MvcApp.Tests;

public sealed class TestHttpClientFactory : IHttpClientFactory
{
    private Dictionary<string, TestDelegatingHandler> _handlerByName = [];

    public TestDelegatingHandler AddHandler(string name)
    {
        TestDelegatingHandler handler = new();
        _handlerByName[name] = handler;
        return handler;
    }

    public static string BaseAddress { get; } = "https://test.app";

    public void RemoveHandler(string name)
    {
        _handlerByName.Remove(name);
    }

    /// <inheritdoc />
    public HttpClient CreateClient(string name)
    {
        return new HttpClient(_handlerByName[name]) // assume it's been added, it'll be a failure if it hasn't anyway
        {
            BaseAddress = new Uri(BaseAddress),
        };
    }
}
