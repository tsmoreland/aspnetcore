using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TennisByTheSea.Domain.Contracts.Bookings.Queries;
using TennisByTheSea.Domain.Contracts.Greetings.Queries;
using TennisByTheSea.Domain.Models;
using TennisByTheSea.MvcApp.Models.Accounts;

namespace TennisByTheSea.MvcApp.Pages;

public class BookingsModel : PageModel
{
    private readonly UserManager<TennisUser> _userManager;
    private readonly IMediator _mediator;

    public BookingsModel(
        UserManager<TennisUser> userManager,
        IMediator mediator)
    {
        _userManager = userManager;
        _mediator = mediator;
    }

    public IEnumerable<IGrouping<DateTime, CourtBooking>> CourtBookings { get; set; } = Array.Empty<IGrouping<DateTime, CourtBooking>>();

    public string Greeting { get; private set; } = "Hello";

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

        Task<(string Greeting, string Colour)> greetingTask =
            _mediator.Send(new GetRandomGreetingQuery($"{user.Member.Forename} {user.Member.Surname}"));

        CourtBookings = (await _mediator
                .CreateStream(new GetFutureBookingsForMemberQuery(user.Member))
                .ToListAsync())
            .GroupBy(b => b.StartDateTime.Date);
        (Greeting, _) = await greetingTask;

        return Page();
    }
}
