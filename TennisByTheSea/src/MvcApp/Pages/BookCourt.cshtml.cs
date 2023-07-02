using System.ComponentModel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TennisByTheSea.Domain.Configuration;
using TennisByTheSea.Domain.Contracts.Bookings.Commands;
using TennisByTheSea.Domain.Contracts.Bookings.Queries;
using TennisByTheSea.Domain.Models;
using TennisByTheSea.MvcApp.Models.Accounts;
using TennisByTheSea.Shared;

namespace TennisByTheSea.MvcApp.Pages;

public class BookCourtModel : PageModel
{
    private readonly UserManager<TennisUser> _userManager;
    private readonly IMediator _mediator;
    private readonly BookingOptions _options;

    public BookCourtModel(
        UserManager<TennisUser> userManager,
        IMediator mediator,
        IOptions<BookingOptions> options)
    {
        _userManager = userManager;
        _mediator = mediator;
        _options = options.Value;
    }

    public string[] Errors { get; set; } = Array.Empty<string>();

    [BindProperty(SupportsGet = true)]
    public int CourtId { get; set; }

    [DisplayName("Booking Length (Hours)")]
    [BindProperty]
    public int BookingLengthInHours { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime BookingStartTime { get; set; }
    public SelectList PossibleHourLengths { get; set; } = new(Array.Empty<int>());

    public async Task OnGet()
    {
        int maxHours = _options.MaxRegularBookingLengthInHours;
        int maxAvailableHour = await _mediator.Send(
            new GetMaxBookingSlotForCourtQuery(BookingStartTime,
                TimeSpan.FromHours(maxHours), CourtId));
        PossibleHourLengths = new SelectList(Enumerable.Range(1, maxAvailableHour));
    }

    public async Task<IActionResult> OnPost()
    {
        if (BookingStartTime < DateTime.UtcNow)
        {
            return new BadRequestResult();
        }

        if (User.Identity is null)
            return new ChallengeResult();

        TennisUser? user = await _userManager.Users
            .Include(u => u.Member)
            .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

        if (user is null)
        {
            return new ChallengeResult();
        }

        OptionalResult<CourtBooking> result = await _mediator.Send(new AddBookingCommand(BookingStartTime,
            TimeSpan.FromHours(BookingLengthInHours), CourtId, user.Member));

        if (result.HasValue)
        {
            TempData["BookingSuccess"] = true;
            return RedirectToPage("/Bookings");
        }

        Errors = result.Errors.ToArray();
        return Page();
    }
}
