using GloboTicket.TicketManagement.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.TicketManagement.Identity;

public class GloboTicketIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    /// <inheritdoc />
    public GloboTicketIdentityDbContext(DbContextOptions options)
        : base(options)
    {
    }

}
