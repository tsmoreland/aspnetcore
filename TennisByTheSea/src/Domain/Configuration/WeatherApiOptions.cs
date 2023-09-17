namespace TennisByTheSea.Domain.Configuration;

public sealed class WeatherApiOptions
{
    public string Url { get; init; } = string.Empty;
    public TimeSpan CachePeriod { get; init; } = TimeSpan.FromSeconds(30);

}
