using MediatR;
using Microsoft.Extensions.Options;
using TennisByTheSea.Domain.Configuration;
using TennisByTheSea.Domain.Contracts.Courts.Queries;
using TennisByTheSea.Domain.Contracts.Services.Unavailability;
using TennisByTheSea.Domain.Models;

namespace TennisByTheSea.Application.Services.Unavailability;

public sealed class OutsideCourtUnavailabilityProvider : IUnavailabilityProvider
{
    private readonly IMediator _mediator;
    private readonly ICollection<int> _outdoorCourtWinterClosedHours;

    private readonly IReadOnlyList<int> _winterMonths;

    public OutsideCourtUnavailabilityProvider(IMediator mediator, IOptions<ClubOptions> options)
    {
        _mediator = mediator;
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
    public async IAsyncEnumerable<HourlyUnavailability> GetHourlyUnavailabilityAsync(DateTime date)
    {
        bool isWinter = _winterMonths.Contains(date.Month);

        if (!isWinter)
        {
            yield break;
        }

        IEnumerable<Court> courts = await _mediator.CreateStream(new GetOutdoorCourtsQuery()).ToListAsync();
        var outsideCourtIds = courts.Select(c => c.Id).ToHashSet();
        IEnumerable<HourlyUnavailability> hourlyUnavailability = _outdoorCourtWinterClosedHours.Select(closedHour => new HourlyUnavailability(closedHour, outsideCourtIds));

        foreach (HourlyUnavailability unavailability in hourlyUnavailability)
        {
            yield return unavailability;
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<int> GetHourlyUnavailabilityAsync(DateTime date, int courtId)
    {
        IEnumerable<Court> courts = await _mediator.CreateStream(new GetOutdoorCourtsQuery()).ToListAsync();

        ICollection<int> unavailableCourts = courts.Select(x => x.Id).Contains(courtId)
            ? _outdoorCourtWinterClosedHours
            : Array.Empty<int>();

        foreach (int id in unavailableCourts)
        {
            yield return id;
        }
    }
}
