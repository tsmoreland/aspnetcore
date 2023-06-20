using TennisByTheSea.Domain.Contracts.Services.Unavailability;
using TennisByTheSea.Domain.Models;

namespace TennisByTheSea.Application.Services.Unavailability;

public sealed class CourtBookingUnavailabilityProvider : IUnavailabilityProvider
{
    //private readonly ICourtBookingService _courtBookingService;

    public CourtBookingUnavailabilityProvider(/*ICourtBookingService courtBookingService*/)
    {
        //_courtBookingService = courtBookingService;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<HourlyUnavailability>> GetHourlyUnavailabilityAsync(DateTime date)
    {
        await Task.CompletedTask;
        IEnumerable<CourtBooking> bookings = Array.Empty<CourtBooking>(); // TODO await _courtBookingService.BookingsForDayAsync(date);

        return BookingsToUnavailability(bookings);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<int>> GetHourlyUnavailabilityAsync(DateTime date, int courtId)
    {
        await Task.CompletedTask;
        IEnumerable<CourtBooking> bookings = Array.Empty<CourtBooking>(); // TODO await _courtBookingService.BookingsForDayAsync(date);

        IEnumerable<HourlyUnavailability> availability = BookingsToUnavailability(bookings);

        return availability.Select(x => x.Hour);
    }

    private static IEnumerable<HourlyUnavailability> BookingsToUnavailability(IEnumerable<CourtBooking> courtBookings)
    {
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

        return unavailability.Where(x => x.UnavailableCourtIds.Any());
    }
}
