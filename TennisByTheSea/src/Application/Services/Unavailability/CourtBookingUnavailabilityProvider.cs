using MediatR;
using TennisByTheSea.Domain.Contracts.Queries.Bookings;
using TennisByTheSea.Domain.Contracts.Services.Unavailability;
using TennisByTheSea.Domain.Extensions;
using TennisByTheSea.Domain.Models;

namespace TennisByTheSea.Application.Services.Unavailability;

public sealed class CourtBookingUnavailabilityProvider : IUnavailabilityProvider
{
    private readonly IMediator _mediator;

    public CourtBookingUnavailabilityProvider(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<HourlyUnavailability> GetHourlyUnavailabilityAsync(DateTime date)
    {
        IEnumerable<CourtBooking> bookings = await _mediator.CreateStream(new GetBookingsForDayQuery(date.ToDateOnly())).ToListAsync();
        await foreach (HourlyUnavailability unvailableHour in BookingsToUnavailability(bookings))
        {
            yield return unvailableHour;
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<int> GetHourlyUnavailabilityAsync(DateTime date, int courtId)
    {
        IEnumerable<CourtBooking> bookings = await _mediator.CreateStream(new GetBookingsForDayQuery(date.ToDateOnly())).ToListAsync();
        IAsyncEnumerable<HourlyUnavailability> availability = BookingsToUnavailability(bookings);

        await foreach (int hour in availability.Select(x => x.Hour))
        {
            yield return hour;
        }
    }

    private static async IAsyncEnumerable<HourlyUnavailability> BookingsToUnavailability(IEnumerable<CourtBooking> courtBookings)
    {
        await Task.CompletedTask;
        List<HourlyUnavailability> unavailability = new();

        for (int i = 0; i < 24; i++)
        {
            unavailability.Add(new HourlyUnavailability(i, new HashSet<int>()));
        }

        foreach (CourtBooking booking in courtBookings)
        {
            for (int i = booking.StartDateTime.Hour; i < booking.EndDateTime.Hour; i++)
            {
                unavailability.First(x => x.Hour == i).UnavailableCourtIds.Add(booking.Court.Id);
            }
        }

        foreach (HourlyUnavailability unavailableHour in unavailability.Where(x => x.UnavailableCourtIds.Any()))
        {
            yield return unavailableHour;
        }
    }
}
