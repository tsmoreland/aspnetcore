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

using GloboTicket.Shop.Shared.Contracts.Persistence.Specifications;
using GloboTicket.Shop.Shared.Models.Persistence;

namespace GloboTicket.Shop.Shared.Contracts.Persistence;

public interface IAsyncReadOnlyRepository<T> where T : class
{
    ValueTask<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    ValueTask<T?> GetByQueryAsync(IQuerySpecification<T> query, CancellationToken cancellationToken = default);
    ValueTask<TProjection?> GetProjectionByQueryAsync<TProjection>(IQuerySpecification<T, TProjection> query, CancellationToken cancellationToken = default);

    IAsyncEnumerable<T> GetAll(CancellationToken cancellationToken = default);
    IAsyncEnumerable<TProjection> GetAll<TProjection>(ISelectorSpecification<T, TProjection> selector, CancellationToken cancellationToken = default);

    ValueTask<Page<T>> GetPage(
        IQuerySpecification<T> query,
        CancellationToken cancellationToken = default);

    ValueTask<Page<TProjection>> GetPage<TProjection>(
        IQuerySpecification<T, TProjection> query,
        CancellationToken cancellationToken = default);
}
