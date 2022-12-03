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

using WiredBrainCoffee.EmployeeManager.Domain.Models;
using WiredBrainCoffee.EmployeeManager.Infrastructure.Entities;

namespace WiredBrainCoffee.EmployeeManager.Infrastructure.Converters;

public static class DepartmentEntityConverter
{
    public static Department Convert(DepartmentEntity entity)
    {
        return Convert(entity, true);
    }

    public static Department Convert(DepartmentEntity entity, bool includeEmployees)
    {
        ArgumentNullException.ThrowIfNull(entity);

        Department department = includeEmployees
            ? new Department(entity.Id, entity.Name, entity.Employees.Select(EmployeeEntityConverter.Convert))
            : new Department(entity.Id, entity.Name);
        return department;
    }

    public static DepartmentEntity Convert(Department model)
    {
        ArgumentNullException.ThrowIfNull(model);

        DepartmentEntity entity = new()
        {
            Name = model.Name,
        };
        if (model.IncludesEmployees)
        {
            entity.Employees = model.Employees.Select(EmployeeEntityConverter.Convert).ToList();
        }

        if (model.Id != 0)
        {
            entity.Id = model.Id;
        }

        return entity;
    }

    public static DepartmentEntity Convert(DepartmentEntity entity, Department model)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(model);

        if (entity.Id != model.Id)
        {
            throw new ArgumentException("Ids do not match", nameof(model));
        }

        entity.Name = model.Name;

        if (!model.IncludesEmployees)
        {
            return entity;
        }

        var employeesById = model.Employees.Select(EmployeeEntityConverter.Convert).ToDictionary(e => e.Id, e => e);

        Dictionary<int, int> employeeEntityPositionById = new();
        foreach ((EmployeeEntity employee, int position) in entity.Employees.Select((value, index) => (value, index)))
        {
            employeeEntityPositionById[employee.Id] = position;
        }
        Dictionary<int, int> employeePositionById = new();
        foreach ((Employee employee, int position) in model.Employees.Select((value, index) => (value, index)))
        {
            employeePositionById[employee.Id] = position;
        }

        List<EmployeeEntity> added = new();
        foreach (Employee employee in model.Employees)
        {
            if (employeeEntityPositionById.TryGetValue(employee.Id, out int index))
            {
                EmployeeEntityConverter.Convert(entity.Employees[index], employee);
            }
            else
            {
                added.Add(EmployeeEntityConverter.Convert(employee));
            }
        }

        List<EmployeeEntity> removed = entity.Employees
            .Where(employeeEntity => !employeePositionById.ContainsKey(employeeEntity.Id))
            .ToList();

        entity.Employees.RemoveAll(removed.Contains);
        entity.Employees.AddRange(added);

        return null!;
    }
}
