using MediatR;
using Microsoft.Extensions.Options;
using TennisByTheSea.Domain.Configuration;
using TennisByTheSea.Domain.Contracts.Queries.Courts;
using TennisByTheSea.Domain.Contracts.Services.Unavailability;
using TennisByTheSea.Domain.Models;

namespace TennisByTheSea.Application.Services.Unavailability;

public sealed class ClubClosedUnavailabilityProvider : IUnavailabilityProvider
{
    private readonly IMediator _mediator;
    private readonly ICollection<int> _closedHours;

    public ClubClosedUnavailabilityProvider(IMediator mediator, IOptions<ClubOptions> options)
    {
        _mediator = mediator;

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
    public async IAsyncEnumerable<HourlyUnavailability> GetHourlyUnavailabilityAsync(DateTime date)
    {
        IEnumerable<int> courtIds = await _mediator.CreateStream(new GetCourtIdsQuery()).ToListAsync();
        IEnumerable<HourlyUnavailability> hourlyUnavailability = _closedHours.Select(closedHour => new HourlyUnavailability(closedHour, courtIds));

        foreach (HourlyUnavailability unavailability in hourlyUnavailability)
        {
            yield return unavailability;
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<int> GetHourlyUnavailabilityAsync(DateTime date, int courtId)
    {
        await Task.CompletedTask;
        foreach (int hour in _closedHours)
        {
            yield return hour;
        }
    }
}
