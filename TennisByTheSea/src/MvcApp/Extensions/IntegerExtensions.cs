namespace TennisByTheSea.MvcApp.Extensions;

public static class IntegerExtensions
{
    public static string To12HourClockString(this int hour)
    {
        string suffix = hour < 12 ? "am" : "pm";

        int finalHour = hour switch
        {
            0 => 12,
            > 12 => hour - 12,
            _ => hour
        };

        return $"{finalHour} {suffix}";
    }
}
