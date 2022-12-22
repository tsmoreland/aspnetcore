﻿//
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

public interface IQueryBuilder<T> where T : class
{
    IQueryBuilder<T> WithNoTracking();
    IQueryBuilder<T> WithFilter(IFilterSpecification<T> filterSpecification);
    IQueryBuilder<T> WithOrderBy<TKey>(IOrderBySpecification<T, TKey> orderBySpecification);
    IQueryBuilder<T> WithThenBy<TKey>(IOrderBySpecification<T, TKey> orderBySpecification);
    IQueryBuilder<T> WithInclusion(IInclusionSpecification<T> inclusion);
    IQueryBuilder<T> WithPaging(PageRequest pageRequest);

    IQuerySpecification<T> Query();
}

public interface IQueryBuilder<T, TProjection>
    where T : class
{
    IQueryBuilder<T, TProjection> WithNoTracking();
    IQueryBuilder<T, TProjection> WithFilter(IFilterSpecification<T> filterSpecification);
    IQueryBuilder<T, TProjection> WithOrderBy<TKey>(IOrderBySpecification<T, TKey> orderBySpecification);
    IQueryBuilder<T, TProjection> WithThenBy<TKey>(IOrderBySpecification<T, TKey> orderBySpecification);
    IQueryBuilder<T, TProjection> WithPaging(PageRequest pageRequest);

    IQuerySpecification<T, TProjection> Query();
}

