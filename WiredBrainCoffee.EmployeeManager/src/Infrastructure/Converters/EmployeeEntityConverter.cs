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

using WiredBrainCoffee.EmployeeManager.Domain.DataTramsferObjects;
using WiredBrainCoffee.EmployeeManager.Domain.Models;
using WiredBrainCoffee.EmployeeManager.Infrastructure.Entities;

namespace WiredBrainCoffee.EmployeeManager.Infrastructure.Converters;

public static class EmployeeEntityConverter 
{
    public static Employee Convert(EmployeeEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (entity.Department is null)
        {
            throw new ArgumentException("invalid entity, Department cannot be null.", nameof(entity));
        }

        Employee employee = new(entity.Id, entity.FirstName, entity.LastName, entity.IsDeveloper, DepartmentEntityConverter.Convert(entity.Department, false), entity);
        return employee;
    }

    public static EmployeeEntity Convert(Employee model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (model.Databacking is EmployeeEntity entity)
        {
            return Convert(entity, model);
        }

        entity = new EmployeeEntity()
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            IsDeveloper = model.IsDeveloper,
            DepartmentId = model.Department.Id,
        };

        return entity;
    }

    public static EmployeeEntity Convert(ChangableEmployeeDto dataTransferObject)
    {
        ArgumentNullException.ThrowIfNull(dataTransferObject);
        if (!dataTransferObject.IsValid())
        {
            throw new ArgumentException("Invalid DTO", nameof(dataTransferObject));
        }

        EmployeeEntity entity = new()
        {
            FirstName = dataTransferObject.FirstName!,
            LastName = dataTransferObject.LastName!,
            IsDeveloper = dataTransferObject.IsDeveloper,
            DepartmentId = dataTransferObject.DepartmentId,
        };
        return entity;
    }

    public static EmployeeEntity Convert(EmployeeEntity entity, Employee model)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(model);

        if (entity.Id != model.Id)
        {
            throw new ArgumentException("entity and model id must match", nameof(model));
        }

        if (model.Department is null)
        {
            throw new ArgumentException("model has null department");
        }

        entity.FirstName = model.FirstName;
        entity.LastName = model.LastName;
        entity.IsDeveloper = model.IsDeveloper;
        entity.DepartmentId = model.Department.Id;


        return entity;
    }
}
