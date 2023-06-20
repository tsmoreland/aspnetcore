using Microsoft.Extensions.Options;
using TennisByTheSea.Domain.Configuration;
using TennisByTheSea.Domain.Contracts.Services.Unavailability;
using TennisByTheSea.Domain.Models;

namespace TennisByTheSea.Application.Services.Unavailability;

public sealed class ClubClosedUnavailabilityProvider : IUnavailabilityProvider
{
    //private readonly ICourtService _courtService;
    private readonly ICollection<int> _closedHours;

    public ClubClosedUnavailabilityProvider(/*ICourtService courtService,*/ IOptions<ClubOptions> options)
    {
        //_courtService = courtService;

        List<int> closedHours = new();

        if (options.Value.OpenHour > 0)
        {
            for (int i = 0; i < options.Value.OpenHour; i++)
            {
                closedHours.Add(i);
            }
        }

        if (options.Value.CloseHour <= 23)
        {
            for (int i = options.Value.CloseHour; i <= 23; i++)
            {
                closedHours.Add(i);
            }
        }

        _closedHours = closedHours;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<HourlyUnavailability>> GetHourlyUnavailabilityAsync(DateTime date)
    {
        await Task.CompletedTask;
        IEnumerable<int> courtIds = Array.Empty<int>(); // TODO await _courtService.GetCourtIds();

        IEnumerable<HourlyUnavailability> hourlyUnavailability = _closedHours.Select(closedHour => new HourlyUnavailability(closedHour, courtIds));

        return hourlyUnavailability;
    }

    /// <inheritdoc />
    public Task<IEnumerable<int>> GetHourlyUnavailabilityAsync(DateTime date, int courtId)
    {
        return Task.FromResult(_closedHours.AsEnumerable());
    }
}
