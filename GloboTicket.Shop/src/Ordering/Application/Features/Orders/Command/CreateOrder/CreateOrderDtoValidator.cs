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

using FluentValidation;
using GloboTicket.Shop.Ordering.Domain.Models;

namespace GloboTicket.Shop.Ordering.Application.Features.Orders.Command.CreateOrder;

public sealed class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
{
    /// <inheritdoc />
    public CreateOrderDtoValidator()
    {
        RuleFor(p => p.Date)
            .NotEmpty().WithMessage("{PropertyName} is required");

        RuleFor(p => p.CustomerName)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(CustomerDetails.MaxNameLength);

        RuleFor(p => p.CustomerEmail)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .EmailAddress()
            .MaximumLength(CustomerDetails.MaxEmailLength);

        RuleFor(p => p.BillingAddressLineOne)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(Address.MaxAddressLineLength);
        RuleFor(p => p.BillingAddressLineTwo)
            .MaximumLength(Address.MaxAddressLineLength);
        RuleFor(p => p.BillingPostCode)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(Address.MaxPostCodeLength);
        RuleFor(p => p.BillingCity)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(Address.MaxCityLength);
        RuleFor(p => p.BillingCountry)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(Address.MaxCountryLength);

        RuleFor(p => p.DeliveryAddressLineOne)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(Address.MaxAddressLineLength);
        RuleFor(p => p.DeliveryAddressLineTwo)
            .MaximumLength(Address.MaxAddressLineLength);
        RuleFor(p => p.DeliveryPostCode)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(Address.MaxPostCodeLength);
        RuleFor(p => p.DeliveryCity)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(Address.MaxCityLength);
        RuleFor(p => p.DeliveryCountry)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(Address.MaxCountryLength);

        RuleFor(p => p.Items)
            .NotEmpty().WithMessage("At least one item must be added")
            .NotNull()
            .Must(((items =>
            {
                List<Guid> ids = new();
                bool valid = items
                    .All(t =>
                    {
                        ids.Add(t.ItemId);
                        return t is { TicketCount: > 0, Price: > 0 };
                    });
                return valid && ids.Count == ids.Distinct().Count();
            }))).WithMessage("Items must be unique with non-zero count and price");
    }
}
