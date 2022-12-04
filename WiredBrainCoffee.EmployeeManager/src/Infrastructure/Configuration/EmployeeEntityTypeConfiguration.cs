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
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WiredBrainCoffee.EmployeeManager.Infrastructure.Entities;

namespace WiredBrainCoffee.EmployeeManager.Infrastructure.Configuration;

public sealed class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<EmployeeEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.IsDeveloper).HasDefaultValue(false);
        builder
            .HasOne(e => e.Department)
            .WithMany(e => e.Employees)
            .HasForeignKey(e => e.DepartmentId);
        builder.Property(e => e.LastModifiedTime).IsRequired();
        builder.Property(e => e.ConcurrencyToken).IsRequired();
    }
}
