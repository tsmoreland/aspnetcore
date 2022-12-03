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

using Microsoft.AspNetCore.Components;
using WiredBrainCoffee.EmployeeManager.Domain.Contracts;
using WiredBrainCoffee.EmployeeManager.Domain.DataTramsferObjects;
using WiredBrainCoffee.EmployeeManager.Domain.Models;
using WiredBrainCoffee.EmployeeManager.Infrastructure.Contracts;
using WiredBrainCoffee.EmployeeManager.Server.App.Shared;

namespace WiredBrainCoffee.EmployeeManager.Server.App.Pages;

public partial class EditEmployee
{
    private string? ErrorMessage { get; set; }

    [Parameter]
    public int EmployeeId { get; set; }

    private EditEmployeeDto? Employee { get; set; }

    private List<Department>? Departments { get; set; }

    private bool IsBusy { get; set; }

    [Inject]
    private IRepositoryFactory RepositoryFactory { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private StateContainer SharedState { get; set; } = null!;

    private Employee? _employee;

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        IsBusy = true;
        try
        {
            await using IDepartmentRepository departmentRepository = RepositoryFactory.BuildDepartmentRepository();

            Departments = await departmentRepository
                .FindPageAsync(1, int.MaxValue, false, true, false, default)
                .ToListAsync(default);

            await using IEmployeeRepository employeeRepository = RepositoryFactory.BuildEmployeeRepository();

            _employee = await employeeRepository.FindByIdAsync(EmployeeId, true, false, default);
            if (_employee is null)
            {
                return;
            }

            Employee = new EditEmployeeDto
            {
                FirstName = _employee.FirstName, LastName = _employee.LastName, IsDeveloper = _employee.IsDeveloper, DepartmentId = _employee.Department.Id
            };
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task OnValidSubmit()
    {
        if (Employee is null || IsBusy || _employee is null || Departments is null)
        {
            return;
        }

        try
        {
            _employee.FirstName = Employee.FirstName!;
            _employee.LastName = Employee.LastName!;
            _employee.IsDeveloper = Employee.IsDeveloper;
            _employee.Department = Departments!.FirstOrDefault(d => d.Id == Employee.DepartmentId)!;


            IsBusy = true;
            await using IEmployeeRepository repository = RepositoryFactory.BuildEmployeeRepository();
            await repository.UpdateEmployeeAsync(_employee, default);

            ErrorMessage = null;
            NavigationManager.NavigateTo($"/employees/list{SharedState.EmployeeOverviewPage}");

        }
        catch (Exception)
        {
            ErrorMessage = "An Error occurred adding the employee";
        }
        finally
        {
            IsBusy = false;
        }
        
    }
    private Task OnInvalidSubmit()
    {
        ErrorMessage = null;
        return Task.CompletedTask;
    }

    private Task OnSubmit(bool valid)
    {
        return valid
            ? OnValidSubmit()
            : OnInvalidSubmit();
    }

    private void OnCancel()
    {
        NavigationManager.NavigateTo($"/employees/list{SharedState.EmployeeOverviewPage}");
    }
}
