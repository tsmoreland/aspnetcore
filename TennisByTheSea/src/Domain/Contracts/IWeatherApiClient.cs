using TennisByTheSea.Shared.WeatherApi;

namespace TennisByTheSea.Domain.Contracts;

public interface IWeatherApiClient
{
    Task<WeatherForecast?> GetWeatherForecastAsync(string city, CancellationToken cancellationToken);
}
