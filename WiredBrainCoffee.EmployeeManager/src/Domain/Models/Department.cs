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

namespace WiredBrainCoffee.EmployeeManager.Domain.Models;

public sealed class Department
{
    private string _name;
    private readonly List<Employee> _employees = new();

    public Department(string name)
        : this(0, name, Array.Empty<Employee>(), false)
    {
    }

    public Department(string name, IEnumerable<Employee> employees)
        : this(0, name, employees, true)
    {
    }

    public Department(int id, string name, IEnumerable<Employee> employees)
        : this(id, name, employees, true)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "id cannot be 0");
        }

        Id = id;
    }
    public Department(int id, string name)
        : this(id, name, Array.Empty<Employee>(), false)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "id cannot be 0");
        }
    }

    private Department(int id, string name, IEnumerable<Employee> employees, bool includesEmployees)
    {
        if (name is not {Length: > 0 and <= 50})
        {
            throw new ArgumentException("Name cannot be empty or more than 50 characters in length");
        }

        ArgumentNullException.ThrowIfNull(employees);

        Id = id;
        _name = name;
        IncludesEmployees = includesEmployees;
        _employees.AddRange(employees.Distinct());
    }


    public int Id { get; }

    public string Name
    {
        get => _name;
        set
        {
            if (value is not {Length: > 0 and <= 50})
            {
                throw new ArgumentException("Name cannot be empty or more than 50 characters in length");
            }

            _name = value;
        }
    }

    public IEnumerable<Employee> Employees => _employees.AsReadOnly();
    public bool IncludesEmployees { get; }
}
