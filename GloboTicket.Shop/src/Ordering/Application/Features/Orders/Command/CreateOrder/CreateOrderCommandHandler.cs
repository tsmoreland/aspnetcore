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

using GloboTicket.Shop.Ordering.Application.Contracts.Infrastructure;
using GloboTicket.Shop.Ordering.Application.Models.Mail;
using GloboTicket.Shop.Ordering.Domain.Models;
using MediatR;

namespace GloboTicket.Shop.Ordering.Application.Features.Orders.Command.CreateOrder;

public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IEmailService _emailService;

    public CreateOrderCommandHandler(IEmailService emailService)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        OrderForCreation order = request.Order;
        Guid orderId = Guid.NewGuid();

        await _emailService.SendEmail(BuildEmailForOrder(order, orderId), cancellationToken);

        return orderId;
    }

    private static Email BuildEmailForOrder(OrderForCreation order, Guid orderId)
    {
        Email mail = new(order.Customer.Email, "New Order Rceived", $"Order: {orderId}");
        return mail;
    }
}
