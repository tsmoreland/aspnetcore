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

using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using WiredBrainCoffee.EmployeeManager.Domain.Contracts;
using WiredBrainCoffee.EmployeeManager.Domain.Models;
using WiredBrainCoffee.EmployeeManager.Infrastructure.Entities;
using static WiredBrainCoffee.EmployeeManager.Infrastructure.Converters.DepartmentEntityConverter;

namespace WiredBrainCoffee.EmployeeManager.Infrastructure.Repositories;

public sealed class DepartmentRepository : IDepartmentRepository
{
    private readonly EmployeeManagerDbContext _dbContext;
    private readonly bool _disposeContext;

    public DepartmentRepository(EmployeeManagerDbContext dbContext)
        : this(dbContext, false)
    {
    }
    internal DepartmentRepository(EmployeeManagerDbContext dbContext, bool disposeContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _disposeContext = disposeContext;
    }

    /// <inheritdoc />
    public Task<Department?> FindByIdAsync(int id, bool includeEmployees, bool track, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<Department> FindPageAsync(int pageNumber, int pageSize,
        bool includeEmployees, bool ascending, bool track,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        // TODO: add flag for tracking
        // TODO: add class to store page number/size that includes a method like ThrowIfInvalid
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "page number must be greater than or equal to 1");
        }
        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "page size must be greater than or equal to 1");
        }

        IQueryable<DepartmentEntity> departmentsQuery = _dbContext.Departments;
        if (!track)
        {
            departmentsQuery = departmentsQuery.AsNoTracking();
        }

        departmentsQuery = departmentsQuery
            .OrderBy(e => e.Name);
        if (includeEmployees)
        {
            departmentsQuery = departmentsQuery.Include(e => e.Employees);
        }

        departmentsQuery = departmentsQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        IAsyncEnumerable<Department> departments = departmentsQuery
            .AsAsyncEnumerable()
            .Select(Convert);

        await foreach (Department department in departments.WithCancellation(cancellationToken))
        {
            yield return department;
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposeContext)
        {
            _dbContext.Dispose();
        }
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        return _disposeContext
            ? _dbContext.DisposeAsync()
            : ValueTask.CompletedTask;
    }

}
