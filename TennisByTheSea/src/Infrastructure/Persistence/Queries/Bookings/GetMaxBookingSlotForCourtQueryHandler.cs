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

using MediatR;
using TennisByTheSea.Domain.Contracts.Bookings.Queries;
using TennisByTheSea.Domain.Extensions;
using TennisByTheSea.Domain.ValueObjects;

namespace TennisByTheSea.Infrastructure.Persistence.Queries.Bookings;

public sealed class GetMaxBookingSlotForCourtQueryHandler : IRequestHandler<GetMaxBookingSlotForCourtQuery, int>
{
    private readonly IMediator _mediator;

    public GetMaxBookingSlotForCourtQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async Task<int> Handle(GetMaxBookingSlotForCourtQuery request, CancellationToken cancellationToken)
    {
        DateTime startTime = request.StartTime;
        DateTime endTime = startTime.Add(request.Duration);
        int courtId = request.CourtId;

        HourlyAvailabilityDictionary hourlyAvailability = await _mediator
            .Send(new GetBookingAvailabilityForDateQuery(startTime.ToDateOnly()),
                cancellationToken);
        IEnumerable<int> hoursToCheck = Enumerable.Range(startTime.Hour, endTime.Hour - startTime.Hour);
        int lastHourAvailable = endTime.Hour;
        foreach (int hourToCheck in hoursToCheck)
        {
            if (hourlyAvailability[hourToCheck][courtId])
            {
                continue;
            }

            lastHourAvailable = hourToCheck;
            break;
        }

        return lastHourAvailable - startTime.Hour;
    }
}
