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

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WiredBrainCoffee.EmployeeManager.Infrastructure.Contracts;
using WiredBrainCoffee.EmployeeManager.Infrastructure.Entities;
using WiredBrainCoffee.EmployeeManager.Shared;
using WiredBrainCoffee.EmployeeManager.Shared.Contracts;

namespace WiredBrainCoffee.EmployeeManager.Infrastructure.Configuration;

public sealed class SqliteModelConfiguration : IModelConfiguration
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SqliteModelConfiguration> _logger;
    private readonly bool _isDevelopment;

    public SqliteModelConfiguration(
        IConfiguration configuration,
        IHostEnvironmentFacade environment,
        ILoggerFactory loggerFactory)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        ArgumentNullException.ThrowIfNull(loggerFactory);
        ArgumentNullException.ThrowIfNull(environment);

        _logger = loggerFactory.CreateLogger<SqliteModelConfiguration>();
        _isDevelopment = environment.IsDevelopment();
    }

    /// <inheritdoc />
    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqliteModelConfiguration).Assembly);

        modelBuilder.Entity<DepartmentEntity>().HasData(
            new DepartmentEntity { Id = 1, Name = "Finance" },
            new DepartmentEntity { Id = 2, Name = "Sales" },
            new DepartmentEntity { Id = 3, Name = "Marketing" },
            new DepartmentEntity { Id = 4, Name = "HR" },
            new DepartmentEntity { Id = 5, Name = "IT" },
            new DepartmentEntity { Id = 6, Name = "Research and Development" });

        modelBuilder.Entity<EmployeeEntity>().HasData(
            new EmployeeEntity { Id = 1, FirstName = "John", LastName = "Smith", DepartmentId = 1 },
            new EmployeeEntity { Id = 2, FirstName = "Jessica", LastName = "Jones", DepartmentId = 2 },
            new EmployeeEntity { Id = 3, FirstName = "Tony", LastName = "Stark", IsDeveloper = true, DepartmentId = 3 },
            new EmployeeEntity { Id = 4, FirstName = "Edward", LastName = "Enigma", DepartmentId = 4 },
            new EmployeeEntity { Id = 5, FirstName = "Brenda", LastName = "Moore", DepartmentId = 5 },
            new EmployeeEntity { Id = 6, FirstName = "Bruce", LastName = "Wayne", IsDeveloper = true, DepartmentId = 6 });

    }

    /// <inheritdoc />
    public void ConfigureContext(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        string connectionString = _configuration
            .GetConnectionString("DefaultConnection") ?? throw new InvalidConfigurationException("missing ApplicationConnection entry.");
        optionsBuilder
            .UseSqlite(
                connectionString,
                options => options.MigrationsAssembly(typeof(SqliteModelConfiguration).Assembly.FullName))
            .LogTo(message => _logger.LogInformation("{SQL}", message))
            .EnableSensitiveDataLogging(_isDevelopment);


        if (!_isDevelopment)
        {
            optionsBuilder.UseModel(CompiledModels.EmployeeManagerDbContextModel.Instance);
        }
    }

    /// <inheritdoc />
    public void SaveChangesVisitor(EmployeeManagerDbContext dbContext)
    {
        IEnumerable<Entity> entries = dbContext.ChangeTracker
            .Entries()
            .Where(e => e.State is EntityState.Modified or EntityState.Added)
            .Select(e => e.Entity)
            .OfType<Entity>();


        foreach (Entity entity in entries)
        {
            entity.LastModifiedTime = DateTimeOffset.UtcNow;
        }
    }
}
