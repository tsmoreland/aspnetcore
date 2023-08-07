using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TennisByTheSea.Domain.Configuration;
using TennisByTheSea.Domain.Contracts;
using TennisByTheSea.Shared.WeatherApi;

namespace TennisByTheSea.Application.BackgroundServices.WeatherCache;

public sealed class WeatherCacheService : BackgroundService
{
    private readonly IWeatherApiClient _weatherApiClient;
    private readonly IDistributedCache<WeatherResult> _cache;
    private readonly IOptionsMonitor<WeatherApiOptions> _options;
    private readonly ILogger<WeatherCacheService> _logger;

    private readonly TimeSpan _cachePeriod;
    private readonly TimeSpan _refreshInterval;

    /// <inheritdoc />
    public WeatherCacheService(
        IWeatherApiClient weatherApiClient,
        IDistributedCache<WeatherResult> cache,
        IOptionsMonitor<WeatherApiOptions> options,
        ILogger<WeatherCacheService> logger)
    {
        _weatherApiClient = weatherApiClient;
        _cache = cache;
        _options = options;
        _logger = logger;

        _cachePeriod = options.CurrentValue.CachePeriod;
        _refreshInterval = CalculateRefreshIntervalFromCachePeriod(_cachePeriod);
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            WeatherForecast? forecast = await _weatherApiClient.GetWeatherForecastAsync("Eastbourne", stoppingToken);
            if (forecast is not null)
            {
                WeatherResult currentWeather = new(forecast.City, forecast.Weather);
                string cacheKey = $"current_weather_{DateTime.UtcNow:yyyy_MM_dd}";
                _logger.LogInformation("updating weather in cache");
                await _cache.SetAsync(cacheKey, currentWeather, _cachePeriod);
            }

            await Task.Delay(_refreshInterval, stoppingToken);
        }
    }

    private static TimeSpan CalculateRefreshIntervalFromCachePeriod(in TimeSpan period)
    {
        return period.TotalMinutes > 1
            ? TimeSpan.FromSeconds((period.TotalMinutes - 1) * 60)
            : TimeSpan.FromSeconds(30);
    }
}
