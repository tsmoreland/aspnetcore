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

using FluentValidation.Results;
using GloboTicket.Shop.Ordering.Domain.Contracts.Infrastructure;
using GloboTicket.Shop.Ordering.Domain.Models;
using GloboTicket.Shop.Ordering.Domain.Models.Mail;
using GloboTicket.Shop.Shared.Contracts.Exceptions;
using MediatR;

namespace GloboTicket.Shop.Ordering.Application.Features.Orders.Command.CreateOrder;

public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IEmailService _emailService;
    private readonly ITelemetryService _telemetryService;

    public CreateOrderCommandHandler(IEmailService emailService, ITelemetryService telemetryService)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        CreateOrderDto orderDto = request.Order;

        ValidationResult validationResult = await new CreateOrderDtoValidator()
            .ValidateAsync(orderDto, cancellationToken)
            .ConfigureAwait(false);
        ValidationFailureException.ThrowIfHasErrors(validationResult);

        OrderForCreation order = orderDto.ToModel();
        Guid orderId = Guid.NewGuid();

        await _emailService
            .SendEmail(BuildEmailForOrder(order, orderId), cancellationToken)
            .ConfigureAwait(false);
        _telemetryService.SendInsights(nameof(CreateOrderCommand), 1);

        return orderId;
    }

    private static Email BuildEmailForOrder(OrderForCreation order, Guid orderId)
    {
        Email mail = new(order.Customer.Email, "New Order Rceived", $"Order: {orderId}");
        return mail;
    }
}
