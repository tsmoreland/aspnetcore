namespace TennisByTheSea.Application.Configuration;

public sealed class HomePageOptions
{
	public bool EnableGreeting { get; set; }
	public bool EnableWeatherForecast { get; set; }
	public string ForecastSectionTitle { get; set; } = string.Empty;
}
