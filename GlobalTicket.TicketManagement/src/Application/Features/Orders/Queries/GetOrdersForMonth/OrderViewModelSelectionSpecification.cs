// 
// Copyright © 2022 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Linq.Expressions;
using GlobalTicket.TicketManagement.Application.Contracts.Persistence;
using GlobalTicket.TicketManagement.Application.Contracts.Persistence.Specifications;
using GlobalTicket.TicketManagement.Domain.Entities;

namespace GlobalTicket.TicketManagement.Application.Features.Orders.Queries.GetOrdersForMonth;

public sealed class OrderViewModelSelectionSpecification : ISelectorSpecification<Order, OrderViewModel>
{
    /// <inheritdoc />
    public Expression<Func<Order, OrderViewModel>> Selector => e => new OrderViewModel(e.Id, e.OrderTotal, e.OrderPlaced);

    /// <inheritdoc />
    public IAsyncEnumerable<OrderViewModel> ProjectToAsyncEnumerable(IQueryable<Order> query, IQueryableToEnumerableConverter queryableConverter)
    {
        return queryableConverter
            .ConvertToAsyncEnumerable(query.Select(e => new { e.Id, e.OrderTotal, e.OrderPlaced }))
            .Select(e => new OrderViewModel(e.Id, e.OrderTotal, e.OrderPlaced));
    }
}
