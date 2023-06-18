namespace TennisByTheSea.Application.BackgroundServices.WeatherCache;

public sealed record class WeatherResult(string City, string? Summary, Wind? Wind, Temperature? Temperature);
