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
//

using System.Linq.Expressions;
using GlobalTicket.TicketManagement.Application.Contracts.Persistence;
using GlobalTicket.TicketManagement.Application.Contracts.Persistence.Specifications;
using GlobalTicket.TicketManagement.Domain.Entities;

namespace GlobalTicket.TicketManagement.Application.Features.Events.Queries.GetEventsExport;

internal sealed class EventExportDtoSelectionSpecification : ISelectorSpecification<Event, EventExportDto>
{
    /// <inheritdoc />
    public Expression<Func<Event, EventExportDto>> Selector => e => new EventExportDto(e.EventId, e.Name, e.Date);

    /// <inheritdoc />
    /// <remarks>
    /// Not entirely convinced the above selector will work, so this is a fall back mechanism
    /// </remarks>
    public IAsyncEnumerable<EventExportDto> ProjectToAsyncEnumerable(IQueryable<Event> query, IQueryableToEnumerableConverter queryableConverter)
    {
        return queryableConverter
            .ConvertToAsyncEnumerable(query.Select(e => new { e.EventId, e.Name, e.Date }))
            .Select(e => new EventExportDto(e.EventId, e.Name, e.Date));
    }
}
