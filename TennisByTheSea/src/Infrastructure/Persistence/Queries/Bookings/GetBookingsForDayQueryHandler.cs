﻿//
// Copyright © 2023 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TennisByTheSea.Domain.Contracts.Bookings.Queries;
using TennisByTheSea.Domain.Models;

namespace TennisByTheSea.Infrastructure.Persistence.Queries.Bookings;

public sealed class GetBookingsForDayQueryHandler : IStreamRequestHandler<GetBookingsForDayQuery, CourtBooking>
{
    private readonly TennisByTheSeaDbContext _dbContext;

    public GetBookingsForDayQueryHandler(TennisByTheSeaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<CourtBooking> Handle(GetBookingsForDayQuery request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        DateTime date = request.Date.ToDateTime(TimeOnly.FromTimeSpan(TimeSpan.Zero));

        DateTime endTime = date.AddMilliseconds(-1);

        IAsyncEnumerable<CourtBooking> bookings = _dbContext.CourtBookings!
            .AsNoTracking()
            .Where(b => b.StartDateTime >= date && b.EndDateTime < endTime)
            .ToAsyncEnumerable();

        await foreach (CourtBooking booking in bookings.WithCancellation(cancellationToken))
        {
            yield return booking;
        }
    }
}
