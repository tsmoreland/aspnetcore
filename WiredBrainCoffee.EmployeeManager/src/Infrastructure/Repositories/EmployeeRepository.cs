﻿//
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
using WiredBrainCoffee.EmployeeManager.Domain.DataTramsferObjects;
using WiredBrainCoffee.EmployeeManager.Domain.Models;
using WiredBrainCoffee.EmployeeManager.Infrastructure.Entities;
using static WiredBrainCoffee.EmployeeManager.Infrastructure.Converters.EmployeeEntityConverter;

namespace WiredBrainCoffee.EmployeeManager.Infrastructure.Repositories;

public sealed class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeManagerDbContext _dbContext;
    private readonly bool _disposeContext;

    public EmployeeRepository(EmployeeManagerDbContext dbContext)
        : this(dbContext, false)
    {
    }
    public EmployeeRepository(EmployeeManagerDbContext dbContext, bool disposeContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _disposeContext = disposeContext;
    }

    /// <inheritdoc />
    public async Task<Employee?> FindByIdAsync(int id, bool includeDepartment, bool track, CancellationToken cancellationToken)
    {
        EmployeeEntity? employeeEntity = null;
        if (track)
        {
            employeeEntity = await _dbContext.Employees.FindAsync(new object[] { id }, cancellationToken);
            if (employeeEntity is null)
            {
                return null;
            }

            if (includeDepartment)
            {
                await _dbContext.Entry(employeeEntity)
                    .Reference(e => e.Department)
                    .LoadAsync(cancellationToken);
            }
        }
        else
        {
            employeeEntity = includeDepartment
                ? await _dbContext.Employees.Include(e => e.Department).FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
                : await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            if (employeeEntity is null)
            {
                return null;
            }
        }

        return Convert(employeeEntity);
    }

    /// <inheritdoc />
    public Task<int> GetTotalCount(CancellationToken cancellationToken)
    {
        return _dbContext.Employees.CountAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<Employee> FindPageAsync(int pageNumber, int pageSize, bool includeDepartment, bool track, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "page number must be greater than or equal to 1");
        }
        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "page size must be greater than or equal to 1");
        }

        IQueryable<EmployeeEntity> employeesQuery = _dbContext.Employees;
        if (!track)
        {
            employeesQuery = employeesQuery.AsNoTracking();
        }


        employeesQuery = employeesQuery
            .OrderBy(e => e.LastName);
        if (!track)
        {
            employeesQuery = employeesQuery.AsNoTracking();
        }

        if (includeDepartment)
        {
            employeesQuery = employeesQuery.Include(e => e.Department);
        }

        employeesQuery = employeesQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        IAsyncEnumerable<Employee> collection = employeesQuery
            .AsAsyncEnumerable()
            .Select(Convert);

        await foreach (Employee employee in collection.WithCancellation(cancellationToken))
        {
            yield return employee;
        }
    }

    /// <inheritdoc />
    public async Task AddEmployeeAsync(AddEmployeeDto employeeDto, CancellationToken cancellationToken)
    {
        EmployeeEntity entity = Convert(employeeDto);

        _dbContext.Employees.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken)
    {
        EmployeeEntity entity = Convert(employee);

        _dbContext.Employees.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateEmployeeAsync(EditEmployeeDto employeeDto, CancellationToken cancellationToken)
    {
        EmployeeEntity entity = Convert(employeeDto);

        if (_dbContext.Entry(entity).State == EntityState.Detached)
        {
            _dbContext.Employees.Update(entity);
        }
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken)
    {
        EmployeeEntity entity = Convert(employee);

        if (_dbContext.Entry(entity).State == EntityState.Detached)
        {
            _dbContext.Employees.Update(entity);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public Task DeleteEmployeeAsync(Employee employee, CancellationToken cancellationToken)
    {
        if (employee == null!)
        {
            return Task.FromException(new ArgumentNullException(nameof(employee)));
        }

        return DeleteEmployeeAsync(employee.Id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteEmployeeAsync(int id, CancellationToken cancellationToken)
    {
        EmployeeEntity? entity = await _dbContext.Employees.FindAsync(new object?[] { id }, cancellationToken);
        if (entity is null)
        {
            return;
        }

        _dbContext.Employees.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
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
