using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TennisByTheSea.Domain.Contracts.Queries.Bookings;
using TennisByTheSea.Domain.Models;
using TennisByTheSea.MvcApp.Models.Accounts;

namespace TennisByTheSea.MvcApp.Pages;

public class BookingsModel : PageModel
{
    private readonly UserManager<TennisUser> _userManager;

    private readonly IMediator _mediator;
    /*
    private readonly IGreetingService _loginGreetingService;
    */

    public BookingsModel(
        UserManager<TennisUser> userManager,
        IMediator mediator
        /*, ICourtBookingService courtBookingService, IGreetingService loginGreetingService*/)
    {
        _userManager = userManager;
        _mediator = mediator;
        //_courtBookingService = courtBookingService;
        //_loginGreetingService = loginGreetingService;
    }

    public IEnumerable<IGrouping<DateTime, CourtBooking>> CourtBookings { get; set; } = Array.Empty<IGrouping<DateTime, CourtBooking>>();

    //public string Greeting { get; private set; } = "Hello";

    [TempData]
    public bool BookingSuccess { get; set; }

    public async Task<IActionResult> OnGet()
    {
        TennisUser? user = await _userManager.Users
            .Include(u => u.Member)
            .FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
        if (user == null)
        {
            return new ChallengeResult();
        }

        //Greeting = _loginGreetingService.GetRandomLoginGreeting($"{user.Member.Forename} {user.Member.Surname}");

        CourtBookings = (await _mediator
                .CreateStream(new GetFutureBookingsForMemberQuery(user.Member))
                .ToListAsync())
            .GroupBy(b => b.StartDateTime.Date);

        return Page();
    }
}
