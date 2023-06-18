using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using TennisByTheSea.Domain.Contracts;
using TennisByTheSea.Shared.WeatherApi;

namespace TennisByTheSea.Infrastructure.ExternalApiClients;

public sealed class WeatherApiClient : IWeatherApiClient
{
    private readonly HttpClient _client;
    private readonly ILogger<WeatherApiClient> _logger;

    public WeatherApiClient(HttpClient client, ILogger<WeatherApiClient> logger)
    {
        _client = client;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<WeatherForecast?> GetWeatherForecastAsync(string city, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(city);

        HttpResponseMessage response = await _client.GetAsync($"/api/v1/weather/{city}", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("weather api returned {StatusCode} when attempting to retrieve weather forecast", response.StatusCode);
            return null;
        }

        return await response.Content.ReadFromJsonAsync<WeatherForecast>(cancellationToken: cancellationToken);
    }
}
