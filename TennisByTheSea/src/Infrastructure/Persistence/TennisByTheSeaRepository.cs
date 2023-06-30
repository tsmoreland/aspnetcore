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

using Microsoft.Extensions.Logging;
using TennisByTheSea.Domain.Contracts;
using TennisByTheSea.Domain.Models;

namespace TennisByTheSea.Infrastructure.Persistence;

public sealed class TennisByTheSeaRepository : ITennisByTheSeaRepository
{
    private readonly TennisByTheSeaDbContext _dbContext;
    private readonly ILogger<TennisByTheSeaRepository> _logger;

    public TennisByTheSeaRepository(TennisByTheSeaDbContext dbContext, ILogger<TennisByTheSeaRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Court?> GetCourtByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        _ = _logger;
        return await _dbContext.Courts.FindAsync(new object?[] { id }, cancellationToken);
    }

    /// <inheritdoc />
    public void AddBooking(CourtBooking booking)
    {
        _dbContext.CourtBookings.Add(booking);
    }

    /// <inheritdoc />
    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
