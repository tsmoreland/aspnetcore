using TennisByTheSea.Domain.Contracts.Services.Unavailability;
using TennisByTheSea.Domain.Models;

namespace TennisByTheSea.Application.Services.Unavailability;

public sealed class UpcomingHoursUnavailabilityProvider : IUnavailabilityProvider
{
    /// <inheritdoc />
    public async Task<IEnumerable<HourlyUnavailability>> GetHourlyUnavailabilityAsync(DateTime date)
    {
        await Task.CompletedTask; // TODO
        IEnumerable<int> courts = Array.Empty<int>(); //await _courtService.GetCourtIds();

        if (date.Date != DateTime.UtcNow.Date)
            return Array.Empty<HourlyUnavailability>();

        int firstHourAvailable = GetFirstHourAvailable();

        List<HourlyUnavailability> unavailableHours = new();

        for (int i = 0; i < firstHourAvailable; i++)
        {
            unavailableHours.Add(new HourlyUnavailability(i, courts));
        }

        return unavailableHours;
    }

    /// <inheritdoc />
    public Task<IEnumerable<int>> GetHourlyUnavailabilityAsync(DateTime date, int courtId)
    {
        if (date.Date != DateTime.UtcNow.Date)
        {
            return Task.FromResult(Enumerable.Empty<int>());
        }

        int firstHourAvailable = GetFirstHourAvailable();

        List<int> unavailableHours = new();

        for (int i = 0; i < firstHourAvailable; i++)
        {
            unavailableHours.Add(i);
        }

        return Task.FromResult(unavailableHours.AsEnumerable());
    }

    private static int GetFirstHourAvailable()
    {
        int firstHourAvailable = DateTime.UtcNow.Hour + 1;
        return DateTime.UtcNow.Minute < 30 ? firstHourAvailable : firstHourAvailable + 1;
    }
}
