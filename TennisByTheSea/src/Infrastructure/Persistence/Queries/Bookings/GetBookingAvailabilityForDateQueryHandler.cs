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
using TennisByTheSea.Domain.Contracts.Courts.Queries;
using TennisByTheSea.Domain.Contracts.Services.Unavailability;
using TennisByTheSea.Domain.Models;
using TennisByTheSea.Domain.ValueObjects;

namespace TennisByTheSea.Infrastructure.Persistence.Queries.Bookings;

public sealed class GetBookingAvailabilityForDateQueryHandler : IRequestHandler<GetBookingAvailabilityForDateQuery, HourlyAvailabilityDictionary>
{
    private readonly IMediator _mediator;
    private readonly IReadOnlyList<IUnavailabilityProvider> _unavailabilityProviders;

    public GetBookingAvailabilityForDateQueryHandler(
        IMediator mediator,
        IEnumerable<IUnavailabilityProvider> unavailabilityProviders)
    {
        _mediator = mediator;
        _unavailabilityProviders = unavailabilityProviders.ToList().AsReadOnly();
    }

    /// <inheritdoc />
    public async Task<HourlyAvailabilityDictionary> Handle(GetBookingAvailabilityForDateQuery request, CancellationToken cancellationToken)
    {
        HourlyAvailabilityDictionary bookingAvailability = await InitializeAvailability(cancellationToken);
        DateTime date = request.Date.ToDateTime(TimeOnly.MinValue);

        foreach (IUnavailabilityProvider provider in _unavailabilityProviders)
        {
            await foreach (HourlyUnavailability unavailability in provider
                               .GetHourlyUnavailabilityAsync(date)
                               .WithCancellation(cancellationToken))
            {
                Dictionary<int, bool> courtUnavailability = bookingAvailability[unavailability.Hour];
                foreach (int courtId in unavailability.UnavailableCourtIds)
                {
                    courtUnavailability[courtId] = false;
                }
            }
        }

        return bookingAvailability;
    }

    private async Task<HourlyAvailabilityDictionary> InitializeAvailability(CancellationToken cancellationToken)
    {
        var bookingAvailability = new HourlyAvailabilityDictionary();

        IReadOnlyList<int> allCourtIds = await _mediator
            .CreateStream(new GetCourtIdsQuery(), cancellationToken)
            .ToListAsync(cancellationToken);

        for (int i = 0; i < 24; i++)
        {
            var availability = new Dictionary<int, bool>();

            foreach (int id in allCourtIds)
            {
                availability[id] = true;
            }

            bookingAvailability[i] = availability;
        }

        return bookingAvailability;
    }
}
