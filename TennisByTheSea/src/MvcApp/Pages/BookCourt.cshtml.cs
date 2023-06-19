using System.ComponentModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using TennisByTheSea.MvcApp.Models.Accounts;
using TennisByTheSea.MvcApp.Models.Configuration;

namespace TennisByTheSea.MvcApp.Pages;

public class BookCourtModel : PageModel
{
    private readonly UserManager<TennisUser> _userManager;
    private readonly BookingOptions _options;

    //private readonly ICourtBookingManager _courtBookingManager;
    //private readonly IBookingService _bookingService;

    public BookCourtModel(
        UserManager<TennisUser> userManager,
        IOptions<BookingOptions> options//,
                                        //ICourtBookingManager courtBookingManager,
        /*IBookingService bookingService*/)
    {
        _userManager = userManager;
        _options = options.Value;
        //_courtBookingManager = courtBookingManager;
        //_bookingService = bookingService;
    }

    public string[] Errors { get; set; } = Array.Empty<string>();

    [BindProperty(SupportsGet = true)]
    public int CourtId { get; set; }

    [DisplayName("Booking Length (Hours)")]
    [BindProperty]
    public int BookingLengthInHours { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime BookingStartTime { get; set; }

    //public SelectList PossibleHourLengths { get; set; } = new SelectList(Array.Empty<int>());

    public async Task OnGet()
    {
        int maxHours = _options.MaxRegularBookingLengthInHours;

        await Task.CompletedTask;
        // TOOD: maybe use mediator instead?
        //var maxAvailableHour = await _bookingService.GetMaxBookingSlotForCourtAsync(BookingStartTime, BookingStartTime.AddHours(maxHours), CourtId);

        //PossibleHourLengths = new SelectList(Enumerable.Range(1, maxAvailableHour));
    }

    public async Task<IActionResult> OnPost()
    {
        if (BookingStartTime < DateTime.UtcNow)
        {
            return new BadRequestResult();
        }

        if (User.Identity is null)
            return new ChallengeResult();

        /*
        consider user manager repository and/or mediator to get this logic out of the OnPost, all of this is a bit too much for the View Model
        var user = await _userManager.Users
                .Include(u => u.Member)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
        */
        TennisUser? user = null;
        await Task.CompletedTask;

        if (user is null)
        {
            return new ChallengeResult();
        }

        /*
        var result = await _courtBookingManager.MakeBookingAsync(BookingStartTime, BookingStartTime.AddHours(BookingLengthInHours), CourtId, user.Member);

        if (result.BookingSuccessful)
        {
            TempData["BookingSuccess"] = true;

            return RedirectToPage("/Bookings");
        }
        */

        //Errors = result.Errors.ToArray();
        return Page();
    }

}
