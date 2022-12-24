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

using GloboTicket.TicketManagement.Application.Contracts.Persistence.Specifications;
using GloboTicket.TicketManagement.Domain.Common;

namespace GloboTicket.TicketManagement.Persistence.Specifications;

public sealed class QueryBuilder<T> : IQueryBuilder<T>
    where T : class
{
    private bool _doNotTrack;
    private IFilterSpecification<T>? _filterSpecification;
    private readonly List<IInclusionSpecification<T>> _inclusionSpecification = new();
    private IOrderBySpecification<T>? _orderBySpecification;
    private IOrderBySpecification<T>? _thenBySpecification;
    private PageRequest? _pageRequest;

    public QueryBuilder()
    {
        Reset();
    }

    public void Reset()
    {
        _doNotTrack = default;
        _filterSpecification = default;
        _inclusionSpecification.Clear();
        _orderBySpecification = default;
        _thenBySpecification = default;
        _pageRequest = default;
    }

    /// <inheritdoc />
    public IQueryBuilder<T> WithNoTracking()
    {
        _doNotTrack = true;
        return this;
    }

    /// <inheritdoc />
    public IQueryBuilder<T> WithFilter(IFilterSpecification<T> filterSpecification)
    {
        _filterSpecification = filterSpecification ?? throw new ArgumentNullException(nameof(filterSpecification));
        return this;
    }

    /// <inheritdoc />
    public IQueryBuilder<T> WithOrderBy(IOrderBySpecification<T> orderBySpecification)
    {
        _orderBySpecification = orderBySpecification ?? throw new ArgumentNullException(nameof(orderBySpecification));
        return this;
    }

    /// <inheritdoc />
    public IQueryBuilder<T> WithThenBy(IOrderBySpecification<T> orderBySpecification)
    {
        _thenBySpecification = orderBySpecification ?? throw new ArgumentNullException(nameof(orderBySpecification));
        return this;
    }

    /// <inheritdoc />
    public IQueryBuilder<T> WithInclusion(IInclusionSpecification<T> inclusion)
    {
        _inclusionSpecification.Add(inclusion);
        return this;
    }

    /// <inheritdoc />
    public IQueryBuilder<T> WithPaging(PageRequest pageRequest)
    {
        _pageRequest = pageRequest ?? throw new ArgumentNullException(nameof(pageRequest));
        return this;
    }

    /// <inheritdoc />
    public IQuerySpecification<T> Query()
    {
        return new QuerySpecification<T>(
            _doNotTrack,
            _inclusionSpecification.Select(i => i.NavigationPropertyName).ToList(),
            _filterSpecification?.Predicate,
            _pageRequest,
            _orderBySpecification,
            _thenBySpecification);
    }

    /// <inheritdoc />
    public IQuerySpecification<T, TProjection> Query<TProjection>(ISelectorSpecification<T, TProjection> selector)
    {
        return new QuerySpecification<T, TProjection>(
            selector,
            _doNotTrack,
            _inclusionSpecification.Select(i => i.NavigationPropertyName).ToList(),
            _filterSpecification?.Predicate,
            _pageRequest,
            _orderBySpecification,
            _thenBySpecification);
    }
}
