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

using System.Linq.Expressions;
using GloboTicket.Shop.Catalog.Domain.Models;
using GloboTicket.Shop.Shared.Contracts.Persistence;
using GloboTicket.Shop.Shared.Contracts.Persistence.Specifications;

namespace GloboTicket.Shop.Catalog.Application.Features.Concerts.Queries.GetConcerts;

public sealed class ConcertDtoSelectorSpecification : ISelectorSpecification<Concert, ConcertSummaryDto>
{
    /// <inheritdoc />
    public Expression<Func<Concert, ConcertSummaryDto>> Selector => c => new ConcertSummaryDto(c);

    /// <inheritdoc />
    public IAsyncEnumerable<ConcertSummaryDto> ProjectToAsyncEnumerable(IQueryable<Concert> query, IQueryableToEnumerableConverter queryableConverter)
    {
        return queryableConverter
            .ConvertToAsyncEnumerable(query.Select(c => new { c.ConcertId, c.Name, c.Artist, c.Price, c.Date, c.Description, c.ImageUrl }))
            .Select(c => new ConcertSummaryDto(c.ConcertId, c.Name, c.Artist, c.Date, c.Price, c.Description, c.ImageUrl));
    }
}
