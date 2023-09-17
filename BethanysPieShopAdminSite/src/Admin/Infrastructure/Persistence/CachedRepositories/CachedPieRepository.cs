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

using BethanysPieShop.Admin.Domain.Contracts;
using BethanysPieShop.Admin.Domain.Models;
using BethanysPieShop.Admin.Domain.Projections;
using BethanysPieShop.Admin.Domain.ValueObjects;

namespace BethanysPieShop.Admin.Infrastructure.Persistence.CachedRepositories;

public sealed class CachedPieRepository : IPieRepository
{
    private readonly IPieRepository _repository;

    public CachedPieRepository(IPieRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public IAsyncEnumerable<Pie> GetAll(PiesOrder orderBy, bool descending)
    {
        return _repository.GetAll(orderBy, descending);
    }

    /// <inheritdoc />
    public IAsyncEnumerable<PieSummary> GetSummaries(PiesOrder orderBy, bool descending)
    {
        return _repository.GetSummaries(orderBy, descending);
    }

    /// <inheritdoc />
    public ValueTask<Page<PieSummary>> GetSummaryPage(PageRequest<PiesOrder> request, CancellationToken cancellationToken)
    {
        return _repository.GetSummaryPage(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task<Pie?> FindById(Guid id, CancellationToken cancellationToken)
    {
        return _repository.FindById(id, cancellationToken);
    }

    /// <inheritdoc />
    public IAsyncEnumerable<PieSummary> Search(string query, Guid? categoryId)
    {
        return _repository.Search(query, categoryId);
    }

    /// <inheritdoc />
    public void SetOriginalConcurrencyToken(Pie entity, byte[] concurrencyToken) => 
        _repository.SetOriginalConcurrencyToken(entity, concurrencyToken);

    /// <inheritdoc />
    public ValueTask Add(Pie entity, CancellationToken cancellationToken)
    {
        return _repository.Add(entity, cancellationToken);
    }

    /// <inheritdoc />
    public ValueTask Update(Pie entity, CancellationToken cancellationToken)
    {
        return _repository.Update(entity, cancellationToken);
    }

    /// <inheritdoc />
    public void Delete(Pie entity)
    {
        _repository.Delete(entity);
    }

    /// <inheritdoc />
    public ValueTask Delete(Guid id, CancellationToken cancellationToken = default)
    {
        return _repository.Delete(id, cancellationToken);
    }

    /// <inheritdoc />
    public ValueTask SaveChanges(CancellationToken cancellationToken)
    {
        return _repository.SaveChanges(cancellationToken);
    }
}
