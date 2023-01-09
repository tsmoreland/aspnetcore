// 
// Copyright (c) 2023 Terry Moreland
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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GloboTicket.TicketManagement.Identity;

public sealed class GloboTicketIdentityDbContextDesignTimeFactory : IDesignTimeDbContextFactory<GloboTicketIdentityDbContext>
{
    /// <inheritdoc />
    public GloboTicketIdentityDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<GloboTicketIdentityDbContext> optionsBuilder = new();
        optionsBuilder.UseSqlite(
            "Data Source=identityDesignTime.db;Pooling=false",
            options => options.MigrationsAssembly(typeof(GloboTicketIdentityDbContext).Assembly.FullName));

        GloboTicketIdentityDbContext dbContext = new(optionsBuilder.Options);
        return dbContext;
    }
}
