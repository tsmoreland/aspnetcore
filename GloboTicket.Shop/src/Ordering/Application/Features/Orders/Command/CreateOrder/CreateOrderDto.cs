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

using GloboTicket.Shop.Ordering.Domain.Models;

namespace GloboTicket.Shop.Ordering.Application.Features.Orders.Command.CreateOrder;

public sealed class CreateOrderDto
{
    public required DateTimeOffset Date { get; init; }

    public required string CustomerName { get; init; }
    public required string CustomerEmail { get; init; }

    public required string BillingAddressLineOne { get; init; }
    public required string? BillingAddressLineTwo { get; init; }
    public required string BillingPostCode { get; init; }
    public required string BillingCity { get; init; }
    public required string BillingCountry { get; init; }

    public required string DeliveryAddressLineOne { get; init; }
    public required string? DeliveryAddressLineTwo { get; init; }
    public required string DeliveryPostCode { get; init; }
    public required string DeliveryCity { get; init; }
    public required string DeliveryCountry { get; init; }

    private IEnumerable<(Guid ConcertId, int TicketCount, int Price)> Items { get; init; } =
        Array.Empty<(Guid, int, int)>();

    /// <summary>
    /// Convert DTO to <see cref="OrderForCreation"/> without validating the parameters
    /// </summary>
    /// <exception cref="ArgumentException">
    /// if one or more properties to do not meet the rquirements of <see cref="OrderForCreation"/>
    /// </exception>
    public OrderForCreation ToOrderForCreationOrThrow()
    {
        OrderForCreation order = new(
            Date,
            new CustomerDetails(
                CustomerName,
                CustomerEmail,
                new Address(DeliveryAddressLineOne, DeliveryAddressLineTwo, DeliveryPostCode, DeliveryCity,
                    DeliveryCountry),
                new Address(BillingAddressLineOne, BillingAddressLineTwo, BillingPostCode, BillingCity,
                    BillingCountry)),
            Items.Select(t => new OrderLine(t.ConcertId, t.TicketCount, t.Price)).ToList().AsReadOnly());
        return order;
    }
}
