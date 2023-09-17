using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using WiredBrainCoffee.EmployeeManager.Domain.Contracts;
using WiredBrainCoffee.EmployeeManager.Domain.DataTramsferObjects;
using WiredBrainCoffee.EmployeeManager.Domain.Models;
using WiredBrainCoffee.EmployeeManager.Server.App.Shared;
using WiredBrainCoffee.EmployeeManager.Shared;

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
    protected override Task OnParametersSetAsync()
    {
        return LoadDataAsync();
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
            NavigationManager.NavigateTo($"/employees/list/{SharedState.EmployeeOverviewPage}");

        }
        catch (DbUpdateConcurrencyException)
        {
            // alternative approach - loop around the UpdateEmployee call and load the entity from the db on each pass, then
            // update the employee with the values we've changed.  Probably something handled in the repository itself, possibly
            // by a method like ForceUpdate which checks what this one changed and only forces those changes
            ErrorMessage = "Employee was modified by another user.";
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
        NavigationManager.NavigateTo($"/employees/list/{SharedState.EmployeeOverviewPage}");
    }

    private async Task LoadDataAsync()
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
}
