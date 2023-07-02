using Microsoft.AspNetCore.Identity;
using TennisByTheSea.Domain.Models;

namespace TennisByTheSea.MvcApp.Models.Accounts;

public class TennisUser : IdentityUser
{
    public required ICollection<CourtBooking> CourtBookings { get; set; } = Array.Empty<CourtBooking>();
    public required Member Member { get; set; }
    public required DateTime LastRequestUtc { get; set; }
}
