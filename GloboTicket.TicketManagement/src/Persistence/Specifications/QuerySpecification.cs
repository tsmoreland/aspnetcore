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
using GloboTicket.TicketManagement.Application.Contracts.Persistence.Specifications;
using GloboTicket.TicketManagement.Domain.Common;
using GloboTicket.TicketManagement.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.TicketManagement.Persistence.Specifications;

public record class QuerySpecification<T>(
    bool DoNotTrack,
    IReadOnlyList<string> Inclusions,
    Expression<Func<T, bool>>? Filter,
    PageRequest? PageRequest,
    IOrderBySpecification<T>? OrderBy,
    IOrderBySpecification<T>? ThenBy) : IQuerySpecification<T> where T : class
{
    /// <inheritdoc />
    public IQueryable<T> ApplySpecifications(IQueryable<T> source)
    {
        if (DoNotTrack)
        {
            source = source.AsNoTracking();
        }

        if (Filter is not null)
        {
            source = source.Where(Filter);
        }

        if (OrderBy is not null)
        {
            source = OrderBy.ApplyOrderBy(source);
        }

        if (ThenBy is not null)
        {
            source = ThenBy.ApplyOrderBy(source);
        }

        if (Inclusions.Any())
        {
            foreach (string inclusion in Inclusions)
            {
                source.Include(inclusion);
            }
        }

        return source;
    }

    public IQueryable<T> ApplyPaging(IQueryable<T> source)
    {
        if (PageRequest is null)
        {
            return source;
        }

        return source
            .Skip((PageRequest.PageNumber - 1) * PageRequest.PageSize)
            .Take(PageRequest.PageSize);
    }

    /// <inheritdoc />
    public int PageNumberOrZero => PageRequest?.PageNumber ?? 0;

    /// <inheritdoc />
    public int PageSizeOrZero => PageRequest?.PageSize ?? 0;

    // TODO: move to extension maethod
}

public sealed record class QuerySpecification<T, TProjection>(
    ISelectorSpecification<T, TProjection> SelectorSpecification,
    bool DoNotTrack,
    IReadOnlyList<string> Inclusions,
    Expression<Func<T, bool>>? Filter,
    PageRequest? PageRequest,
    IOrderBySpecification<T>? OrderBy,
    IOrderBySpecification<T>? ThenBy)
    : QuerySpecification<T>(DoNotTrack, Inclusions, Filter, PageRequest, OrderBy, ThenBy)
    , IQuerySpecification<T, TProjection> where T : class
{
    /// <inheritdoc />
    public IQueryable<TProjection> ApplySelection(IQueryable<T> source)
    {
        return source.Select(SelectorSpecification.Selector);
    }

    /// <inheritdoc />
    public IAsyncEnumerable<TProjection> ProjectToAsyncEnumerable(IQueryable<T> source)
    {
        return SelectorSpecification
            .ProjectToAsyncEnumerable(source, new QueryableToEnumerableConverter());
    }
}
