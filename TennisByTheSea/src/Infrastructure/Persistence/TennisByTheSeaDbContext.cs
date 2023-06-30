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

using Microsoft.EntityFrameworkCore;
using TennisByTheSea.Domain.Models;

namespace TennisByTheSea.Infrastructure.Persistence;

public sealed class TennisByTheSeaDbContext : DbContext
{
    private readonly IModelConfiguration _modelConfiguration;

    /// <inheritdoc />
    public TennisByTheSeaDbContext(DbContextOptions<TennisByTheSeaDbContext> options, IModelConfiguration modelConfiguration)
        : base(options)
    {
        _modelConfiguration = modelConfiguration;
    }

	public DbSet<Court> Courts => Set<Court>();

	public DbSet<CourtBooking> CourtBookings => Set<CourtBooking>();

	public DbSet<Member> Members => Set<Member>();

    // TODO:
	//public DbSet<CourtMaintenanceSchedule> CourtMaintenance => Set<CourtMaintenanceSchedule>();

	//public DbSet<ConfigurationEntry> ConfigurationEntries => Set<ConfigurationEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        _modelConfiguration.ConfigureModel(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        _modelConfiguration.ConfigureContext(optionsBuilder);
    }
}
