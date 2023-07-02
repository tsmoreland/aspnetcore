using MediatR;
using Microsoft.EntityFrameworkCore;
using TennisByTheSea.Domain.Contracts.Bookings.Queries;
using TennisByTheSea.Domain.Contracts.Services;
using TennisByTheSea.Domain.Models;

namespace TennisByTheSea.Infrastructure.Persistence.Queries.Bookings;

public sealed class GetFutureBookingsForMemberQueryHandler : IStreamRequestHandler<GetFutureBookingsForMemberQuery, CourtBooking>
{
    private readonly TennisByTheSeaDbContext _dbContext;
    private readonly ITimeService _timeService;

    public GetFutureBookingsForMemberQueryHandler(TennisByTheSeaDbContext dbContext, ITimeService timeService)
    {
        _dbContext = dbContext;
        _timeService = timeService;
    }

    /// <inheritdoc />
    public IAsyncEnumerable<CourtBooking> Handle(GetFutureBookingsForMemberQuery request, CancellationToken cancellationToken)
    {
        Member member = request.Member;
        DateTime currentDate = _timeService.CurrentUtcTime;

        return _dbContext.CourtBookings.AsNoTracking()
            .Where(c => c.Member == member && c.StartDateTime >= currentDate)
            .OrderBy(x => x.StartDateTime)
            .AsAsyncEnumerable();
    }
}
