using MediatR;
using TennisByTheSea.Domain.Contracts.Requests;
using TennisByTheSea.Domain.Contracts.Services.Unavailability;
using TennisByTheSea.Domain.Models;

namespace TennisByTheSea.Application.Services.Unavailability;

public sealed class UpcomingHoursUnavailabilityProvider : IUnavailabilityProvider
{
    private readonly IMediator _mediator;

    public UpcomingHoursUnavailabilityProvider(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<HourlyUnavailability> GetHourlyUnavailabilityAsync(DateTime date)
    {
        await Task.CompletedTask; // TODO
        IEnumerable<int> courts = await _mediator.CreateStream(new GetCourtIdsQuery()).ToListAsync();

        if (date.Date != DateTime.UtcNow.Date)
        {
            yield break;
        }

        int firstHourAvailable = GetFirstHourAvailable();

        for (int i = 0; i < firstHourAvailable; i++)
        {
            yield return new HourlyUnavailability(i, courts);
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<int> GetHourlyUnavailabilityAsync(DateTime date, int courtId)
    {
        if (date.Date != DateTime.UtcNow.Date)
        {
            yield break;
        }

        int firstHourAvailable = GetFirstHourAvailable();

        await Task.CompletedTask;
        for (int i = 0; i < firstHourAvailable; i++)
        {
            yield return i;
        }
    }

    private static int GetFirstHourAvailable()
    {
        int firstHourAvailable = DateTime.UtcNow.Hour + 1;
        return DateTime.UtcNow.Minute < 30 ? firstHourAvailable : firstHourAvailable + 1;
    }
}
