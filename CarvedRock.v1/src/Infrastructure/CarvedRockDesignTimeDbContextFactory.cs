﻿//
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

using CarvedRock.Application.Contracts;
using CarvedRock.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CarvedRock.Infrastructure;

public sealed class CarvedRockDesignTimeDbContextFactory : IDesignTimeDbContextFactory<CarvedRockDbContext>
{
    /// <inheritdoc />
    public CarvedRockDbContext CreateDbContext(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder().Build();

        using ILoggerFactory loggerFactory = new LoggerFactory();
        SqlServerModelConfiguration modelConfiguration = new(configuration, new HostEnvironmentFacade(), loggerFactory.CreateLogger<SqlServerModelConfiguration>());

        DbContextOptionsBuilder<CarvedRockDbContext> optionsBuilder = new();
        // SqlServer 2019 Localdb
        optionsBuilder.UseSqlServer(
            @"Server=(localdb)\MSSQLLocalDB; Integrated Security=True; MultipleActiveResultSets=True",
            options => options.MigrationsAssembly(typeof(SqlServerModelConfiguration).Assembly.FullName));
        CarvedRockDbContext dbContext = new(optionsBuilder.Options, modelConfiguration);
        return dbContext;
    }

    private sealed class HostEnvironmentFacade : IHostEnvironmentFacade
    {
        /// <inheritdoc />
        public bool IsDevelopment => false;
    }
}
