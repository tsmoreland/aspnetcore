using Microsoft.Extensions.Options;
using TennisByTheSea.Domain.Configuration;
using TennisByTheSea.Domain.Contracts;
using TennisByTheSea.Domain.Contracts.Services.Weather;
using TennisByTheSea.Shared.WeatherApi;

namespace TennisByTheSea.Application.Services.Weather;

public sealed class CachedWeatherForecaster : IWeatherForecaster
{
    private readonly IWeatherForecaster _weatherForecaster;
    private readonly IDistributedCache<WeatherResult> _cache;
    private readonly IOptionsMonitor<WeatherApiOptions> _optionsMonitor;

    public CachedWeatherForecaster(
        IWeatherForecaster weatherForecaster,
        IDistributedCache<WeatherResult> cache,
        IOptionsMonitor<WeatherApiOptions> optionsMonitor)
    {
        _weatherForecaster = weatherForecaster;
        _cache = cache;
        _optionsMonitor = optionsMonitor;
    }

    /// <inheritdoc />
    public bool ForecastEnabled => _weatherForecaster.ForecastEnabled;

    /// <inheritdoc />
    public async Task<WeatherResult> GetCurrentWeatherAsync(string city)
    {
        string cacheKey = $"current_weather_{DateTime.UtcNow:yyyy_MM_dd}";
        (bool isCached, WeatherResult? forecast) = await _cache.TryGetValueAsync(cacheKey);
        if (isCached)
        {
            return forecast!;
        }

        WeatherResult weatherResult = await _weatherForecaster.GetCurrentWeatherAsync(city);
        await _cache.SetAsync(cacheKey, weatherResult, _optionsMonitor.CurrentValue.CachePeriod);
        return weatherResult;
    }
}
