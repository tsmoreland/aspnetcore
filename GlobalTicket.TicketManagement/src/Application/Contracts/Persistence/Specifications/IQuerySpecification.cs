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

using GlobalTicket.TicketManagement.Domain.Common;

namespace GlobalTicket.TicketManagement.Application.Contracts.Persistence.Specifications;

public interface IQuerySpecification<T> where T : class
{
    IQuerySpecification<T> WithNoTracking();
    IQuerySpecification<T> WithFilter(IFilterSpecification<T> filterSpecification);
    IQuerySpecification<T> WithOrderBy(IOrderBySpecification<T> orderBySpecification);
    IQuerySpecification<T> WithThenBy(IOrderBySpecification<T> orderBySpecification);
    IQuerySpecification<T> WithPaging(PageRequest pageRequest);

    IQueryable<T> Query();
}

public interface IQuerySpecification<T, out TProjection>
    where T : class
{
    IQuerySpecification<T, TProjection> WithNoTracking();
    IQuerySpecification<T, TProjection> WithFilter(IFilterSpecification<T> filterSpecification);
    IQuerySpecification<T, TProjection> WithOrderBy(IOrderBySpecification<T> orderBySpecification);
    IQuerySpecification<T, TProjection> WithThenBy(IOrderBySpecification<T> orderBySpecification);
    IQuerySpecification<T, TProjection> WithPaging(PageRequest pageRequest);

    IQueryable<TProjection> Query();
}
