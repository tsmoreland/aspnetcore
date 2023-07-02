using System.ComponentModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TennisByTheSea.Domain.Contracts.Bookings.Queries;
using TennisByTheSea.Domain.Extensions;
using TennisByTheSea.Domain.ValueObjects;
using TennisByTheSea.MvcApp.Extensions;

namespace TennisByTheSea.MvcApp.Pages;

public sealed class FindAvailableCourtsModel : PageModel
{
    private readonly IMediator _mediator;


    public FindAvailableCourtsModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    [DisplayName("Search Date")]
    [BindProperty(SupportsGet = true)]
    public DateTime SearchDate { get; set; }

    public IEnumerable<TableData> Availability { get; set; } = Enumerable.Empty<TableData>();

    public bool HasNoAvailability => !Availability.Any();

    public async Task OnGet()
    {
        if (SearchDate == default)
        {
            SearchDate = DateTime.UtcNow.Date;
        }

        await LoadAvailability();
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("FindAvailableCourts", new { searchDate = SearchDate.ToString("yyyy-MM-dd") });
    }

    private async Task LoadAvailability()
    {
        if (SearchDate.Date < DateTime.UtcNow.Date)
        {
            Availability = Array.Empty<TableData>();
            return;
        }

        HourlyAvailabilityDictionary availability = await _mediator
            .Send(new GetBookingAvailabilityForDateQuery(SearchDate.ToDateOnly()));

        List<TableData> tableData = new(24);

        bool foundFirstRow = false;
        int lastHourWithAvailability = 23;

        foreach ((int key, Dictionary<int, bool>? value) in availability)
        {
            bool noAvailability = !value.ContainsValue(true);

            if (noAvailability && !foundFirstRow)
            {
                continue; // skip rows before first availability
            }

            foundFirstRow = true;

            if (!noAvailability)
            {
                lastHourWithAvailability = key;
            }

            TableData data = new(key, SearchDate, value);

            tableData.Add(data);
        }

        Availability = tableData.Where(x => x.Hour <= lastHourWithAvailability);
    }

    public sealed class TableData
    {
        public TableData(int hour, DateTime date, Dictionary<int, bool> courtAvailability)
        {
            if (hour is < 0 or > 23)
            {
                throw new ArgumentOutOfRangeException(nameof(hour));
            }

            ArgumentNullException.ThrowIfNull(courtAvailability);

            Hour = hour;
            HourText = hour.To12HourClockString();
            BookingStartDate = date.Date.AddHours(hour).ToString("O");
            CourtAvailability = courtAvailability.Select(v => new CourtData(v.Key, v.Value));
        }

        public int Hour { get; }

        public string HourText { get; }

        public string BookingStartDate { get; }

        public IEnumerable<CourtData> CourtAvailability { get; }
    }

    public sealed record class CourtData(int CourtId, bool Available);
}
