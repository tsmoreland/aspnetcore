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

using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SunDoeCoffeeShop.Shared.AuthPersistence;

public sealed class UserRepository : IUserRepository
{
    private readonly AuthDbContext _dbContext;

    /// <summary>
    /// Instantiates a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="dbContext"><see cref="AuthDbContext"/> used to interact with the database</param>
    /// <exception cref="ArgumentNullException">if <paramref name="dbContext"/> is <see langword="null"/></exception>
    public UserRepository(AuthDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<IdentityUser> GetAll([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (IdentityUser user in _dbContext.Users.AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            yield return user;
        }
    }

    /// <inheritdoc />
    public Task<IdentityUser?> GetUserById(string id, CancellationToken cancellationToken)
    {
        return _dbContext.Users.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public Task<IdentityUser?> GetUserByUsername(string username, CancellationToken cancellationToken)
    {
        username = username.ToUpperInvariant();
        return _dbContext.Users.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(i => i.NormalizedUserName == username, cancellationToken);
    }

    /// <inheritdoc />
    public Task<IdentityUser?> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        email = email.ToUpperInvariant();
        return _dbContext.Users.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(i => i.NormalizedEmail == email, cancellationToken);
    }
}
