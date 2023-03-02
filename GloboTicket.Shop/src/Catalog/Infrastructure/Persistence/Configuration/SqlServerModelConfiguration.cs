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

using GloboTicket.Shop.Shared.Contracts.Exceptions;
using GloboTicket.Shop.Shared.Contracts.Hosting;
using GloboTicket.Shop.Shared.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GloboTicket.Shop.Catalog.Infrastructure.Persistence.Configuration;

public sealed class SqlServerModelConfiguration : IModelConfiguration<ModelBuilder, DbContextOptionsBuilder>
{
    private readonly IConfiguration _configuration;
    private readonly bool _isDevelopment;
    private readonly ILogger<SqlServerModelConfiguration> _logger;

    public SqlServerModelConfiguration(IConfiguration configuration, IHostEnvironmentFacade environment, ILoggerFactory loggerFactory)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        ArgumentNullException.ThrowIfNull(environment);
        ArgumentNullException.ThrowIfNull(loggerFactory);

        _isDevelopment = environment.IsDevelopment;
        _logger = loggerFactory.CreateLogger<SqlServerModelConfiguration>();
    }

    /// <inheritdoc />
    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventCatalogDbContext).Assembly);
    }

    /// <inheritdoc />
    public void ConfigureContext(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        string connectionString = _configuration
            .GetConnectionString("Default") ?? throw new InvalidConfigurationException("missing connection entry.");
        optionsBuilder
            .UseSqlServer(
                connectionString,
                options => options.MigrationsAssembly(typeof(SqlServerModelConfiguration).Assembly.FullName))
            .LogTo(message => _logger.LogInformation("{SQL}", message))
            .EnableSensitiveDataLogging(_isDevelopment);


        if (!_isDevelopment)
        {
            optionsBuilder.UseModel(CompiledModels.EventCatalogDbContextModel.Instance);
        }
    }
}
