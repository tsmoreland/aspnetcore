using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TennisByTheSea.MvcApp.Pages;

public sealed class FindAvailableCourtsModel : PageModel
{
/*
    private readonly IBookingService _bookingService;

    public FindAvailableCourtsModel(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [DisplayName("Search Date")]
    [BindProperty(SupportsGet = true)]
    public DateTime SearchDate { get; set; }

    public IEnumerable<TableData> Availability { get; set; } = Enumerable.Empty<TableData>();

    public bool HasNoAvailability => !Availability.Any();

    public async Task OnGet()
    {
        if (SearchDate == default)
            SearchDate = DateTime.UtcNow.Date;

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

        var availability = await _bookingService.GetBookingAvailabilityForDateAsync(SearchDate);

        var tableData = new List<TableData>(24);

        var foundFirstRow = false;
        var lastHourWithAvailability = 23;

        foreach (var hour in availability)
        {
            var noAvailability = !hour.Value.ContainsValue(true);

            if (noAvailability && !foundFirstRow)
                continue; // skip rows before first availability

            foundFirstRow = true;

            if (!noAvailability)
                lastHourWithAvailability = hour.Key;

            var data = new TableData(hour.Key, SearchDate, hour.Value);

            tableData.Add(data);
        }

        Availability = tableData.Where(x => x.Hour <= lastHourWithAvailability);
    }

    public class TableData
    {
        public TableData(int hour, DateTime date, Dictionary<int, bool> courtAvailability)
        {
            if (hour < 0 || hour > 23)
                throw new ArgumentOutOfRangeException(nameof(hour));

            if (courtAvailability == null)
                throw new ArgumentNullException(nameof(courtAvailability));

            Hour = hour;
            HourText = hour.To12HourClockString();
            BookingStartDate = date.Date.AddHours(hour).ToString("O");
            CourtAvailability = courtAvailability.Select(v => new CourtData { CourtId = v.Key, Available = v.Value });
        }

        public int Hour { get; }

        public string HourText { get; }

        public string BookingStartDate { get; }

        public IEnumerable<CourtData> CourtAvailability { get; }
    }

    public class CourtData
    {
        public int CourtId { get; set; }
        public bool Available { get; set; }
    }
*/
}
