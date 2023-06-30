namespace TennisByTheSea.Domain.Models;

public class HourlyUnavailability
{
    public HourlyUnavailability(int hour, IEnumerable<int> unavailableCourtIds)
    {
        if (hour is < 0 or > 23)
        {
            throw new ArgumentException("The hour must be between 0 and 23", nameof(hour));
        }

        Hour = hour;
        ArgumentNullException.ThrowIfNull(unavailableCourtIds);
        UnavailableCourtIds = unavailableCourtIds as HashSet<int> ?? unavailableCourtIds.ToHashSet();
    }

    public int Hour { get; }

    public HashSet<int> UnavailableCourtIds { get; }
}
