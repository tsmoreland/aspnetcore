using TennisByTheSea.Domain.Contracts;
using TennisByTheSea.Domain.Contracts.Services.Weather;
using TennisByTheSea.Shared.WeatherApi;

namespace TennisByTheSea.Application.Services.Weather;

public sealed class WeatherForecaster : IWeatherForecaster
{
    private readonly IWeatherApiClient _weatherApiClient;

    public WeatherForecaster(IWeatherApiClient weatherApiClient)
    {
        _weatherApiClient = weatherApiClient;
    }

    /// <inheritdoc />
    public bool ForecastEnabled => true;

    /// <inheritdoc />
    public async Task<WeatherResult> GetCurrentWeatherAsync(string city)
    {
        WeatherForecast? forecast = await _weatherApiClient.GetWeatherForecastAsync(city, default);
        if (forecast is null)
        {
            return WeatherResult.Unkonwn();
        }

        WeatherResult result = new(forecast.City, forecast.Weather);
        return result;
    }
}
