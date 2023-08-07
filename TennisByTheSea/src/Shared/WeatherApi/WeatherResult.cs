namespace TennisByTheSea.Shared.WeatherApi;

public sealed record class WeatherResult(
    string City,
    WeatherConditions? Weather)
{
    public WeatherResult DeepCopy()
    {
        return new WeatherResult(City, Weather?.DeepCopy());
    }

    public static WeatherResult Unkonwn()
    {
        return new WeatherResult("UNKOWN", WeatherConditions.Unkown());
    }
}
