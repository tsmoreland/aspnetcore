using Microsoft.Extensions.Options;
using TennisByTheSea.Domain.Configuration;
using TennisByTheSea.Domain.Contracts.Services.Unavailability;
using TennisByTheSea.Domain.Models;

namespace TennisByTheSea.Application.Services.Unavailability;

public sealed class OutsideCourtUnavailabilityProvider : IUnavailabilityProvider
{
    //private readonly ICourtService _courtService;
    private readonly ICollection<int> _outdoorCourtWinterClosedHours;

    private readonly IReadOnlyList<int> _winterMonths;

    public OutsideCourtUnavailabilityProvider(/* ICourtService courtService, */IOptions<ClubOptions> options)
    {
        //_courtService = courtService;
        _winterMonths = options.Value.WinterMonths.ToList().AsReadOnly();

        List<int> outdoorCourtWinterClosedHours = new();

        if (options.Value.WinterCourtStartHour > 0 && options.Value.WinterCourtStartHour > options.Value.OpenHour)
        {
            for (int i = 0; i < options.Value.WinterCourtStartHour; i++)
            {
                outdoorCourtWinterClosedHours.Add(i);
            }
        }

        if (options.Value.WinterCourtEndHour <= 23 && options.Value.WinterCourtEndHour < options.Value.CloseHour)
        {
            for (int i = options.Value.WinterCourtEndHour; i <= 23; i++)
            {
                outdoorCourtWinterClosedHours.Add(i);
            }
        }

        _outdoorCourtWinterClosedHours = outdoorCourtWinterClosedHours;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<HourlyUnavailability>> GetHourlyUnavailabilityAsync(DateTime date)
    {
        bool isWinter = _winterMonths.Contains(date.Month);

        if (!isWinter)
            return Array.Empty<HourlyUnavailability>();

        await Task.CompletedTask;
        // TODO var courts = await _courtService.GetOutdoorCourts();
        IEnumerable<Court> courts = Array.Empty<Court>();
        var outsideCourtIds = courts.Select(c => c.Id).ToHashSet();

        IEnumerable<HourlyUnavailability> hourlyUnavailability = _outdoorCourtWinterClosedHours.Select(closedHour => new HourlyUnavailability(closedHour, outsideCourtIds));

        return hourlyUnavailability;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<int>> GetHourlyUnavailabilityAsync(DateTime date, int courtId)
    {
        await Task.CompletedTask;
        // TODO var courts = await _courtService.GetOutdoorCourts();
        IEnumerable<Court> courts = Array.Empty<Court>();

        return courts.Select(x => x.Id).Contains(courtId) ? _outdoorCourtWinterClosedHours : Array.Empty<int>();
    }
}
