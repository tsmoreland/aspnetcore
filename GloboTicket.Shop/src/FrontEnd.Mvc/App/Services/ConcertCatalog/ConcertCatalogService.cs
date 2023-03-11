using System.Runtime.CompilerServices;
using GloboTicket.FrontEnd.Mvc.App.Extensions;
using GloboTicket.FrontEnd.Mvc.App.Models.Api;

namespace GloboTicket.FrontEnd.Mvc.App.Services.ConcertCatalog;

public sealed class ConcertCatalogService : IConcertCatalogService
{
    private readonly HttpClient _client;

    public ConcertCatalogService(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<Concert> GetAll([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await _client.GetAsync("api/concerts", cancellationToken);
        await foreach (Concert concert in response.StreamResponseAs<Concert>(cancellationToken))
        {
            yield return concert;
        }
    }

    /// <inheritdoc />
    public async Task<Concert?> GetConcert(Guid id, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await _client.GetAsync($"api/concerts/{id}", cancellationToken);
        return await response.ReadAs<Concert>(cancellationToken);
    }
}
