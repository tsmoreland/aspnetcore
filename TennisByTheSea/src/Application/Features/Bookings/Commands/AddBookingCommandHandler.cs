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
using Microsoft.Extensions.Logging;
using TennisByTheSea.Domain.Contracts;
using TennisByTheSea.Domain.Contracts.Bookings.Commands;
using TennisByTheSea.Domain.Models;
using TennisByTheSea.Shared;

namespace TennisByTheSea.Application.Features.Bookings.Commands;

public sealed class AddBookingCommandHandler : IRequestHandler<AddBookingCommand, OptionalResult<CourtBooking>>
{
    private readonly ITennisByTheSeaRepository _repository;
    private readonly ILogger<AddBookingCommandHandler> _logger;

    public AddBookingCommandHandler(ITennisByTheSeaRepository repository, ILogger<AddBookingCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<OptionalResult<CourtBooking>> Handle(AddBookingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Court? court = await _repository.GetCourtByIdAsync(request.CourtId, cancellationToken);
            if (court is null)
            {
                return OptionalResult.Empty<CourtBooking>($"court {request.CourtId} not found");
            }

            CourtBooking booking = CourtBooking
                .BookCourtForMember(court, request.Member, request.StartDateTime, request.Duration);

            // TODO: use court booking validator via FluentValidation

            _repository.AddBooking(booking);

            await _repository.SaveAsync(cancellationToken);

            await NotifyBookingConfirmed();

            return OptionalResult.Of(booking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred creating booking");
            return OptionalResult.Empty<CourtBooking>(ex.Message);
        }

        /*
		var courtBooking = new CourtBooking
		{
			CourtId = courtId,
			Member = member,
			StartDateTime = startDateTime,
			EndDateTime = endDateTime
		};

		var (passedRules, errors) = await _bookingRuleProcessor.PassesAllRulesAsync(courtBooking);

		if (!passedRules)
			return CourtBookingResult.Failure(errors);

		await _bookingService.CreateCourtBooking(courtBooking);

		await _notificationService.SendAsync("Thank you. Your booking is confirmed", member.User!.Id);

		return CourtBookingResult.Success(courtBooking);
         */
    }

    public Task NotifyBookingConfirmed()
    {
        return Task.CompletedTask;
    }
}
