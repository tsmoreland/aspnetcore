//
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TennisByTheSea.Domain.Contracts.Queries.Bookings;
using TennisByTheSea.Domain.ValueObjects;

namespace TennisByTheSea.Infrastructure.Persistence.Queries.Bookings;

public sealed class GetMaxBookingSlotForCourtQueryHandler : IRequestHandler<GetMaxBookingSlotForCourtQuery, int>
{
    private readonly TennisByTheSeaDbContext _dbContext;

    public GetMaxBookingSlotForCourtQueryHandler(TennisByTheSeaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<int> Handle(GetMaxBookingSlotForCourtQuery request, CancellationToken cancellationToken)
    {
        DateTime startTime = request.StartTime;
        await Task.CompletedTask;

        /*
        var hourlyAvailability = await GetBookingAvailabilityForDateAsync(startTime.Date);

        var hoursToCheck = Enumerable.Range(startTime.Hour, endTime.Hour - startTime.Hour);

        var lastHourAvailable = endTime.Hour;

        foreach (var hourToCheck in hoursToCheck)
        {
            if (hourlyAvailability[hourToCheck][courtId])
                continue;

            lastHourAvailable = hourToCheck;
            break;
        }
        */

        return lastHourAvailable - startTime.Hour;

    }

    /*
    // TODO:
    // move this to a separate request and maybe let it be called as a separate request.  
    // alternatively make it a static internal method that can be called by this class
    public async Task<HourlyAvailabilityDictionary> GetBookingAvailabilityForDateAsync(DateTime date)
    {
        var bookingAvailability = await InitialiseAvailability();

        foreach (var provider in _unavailabilityProviders)
        {
            var hourlyUnavailability = await provider.GetHourlyUnavailabilityAsync(date);

            foreach (var unavailability in hourlyUnavailability)
            {
                var courtUnavailability = bookingAvailability[unavailability.Hour];

                foreach (var courtId in unavailability.UnavailableCourtIds)
                {
                    courtUnavailability[courtId] = false;
                }
            }
        }

        return bookingAvailability;
    }
    */
}
