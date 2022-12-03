using Microsoft.AspNetCore.Components;
using WiredBrainCoffee.EmployeeManager.Domain.Contracts;
using WiredBrainCoffee.EmployeeManager.Domain.DataTramsferObjects;
using WiredBrainCoffee.EmployeeManager.Domain.Models;

namespace WiredBrainCoffee.EmployeeManager.Server.App.Pages;

public partial class AddEmployee
{
    private string? SuccessMessage { get; set; }
    private string? ErrorMessage { get; set; }
    private bool IsBusy { get; set; }

    private AddEmployeeDto? Employee { get; set; }

    private List<Department>? Departments { get; set; }

    [Inject]
    public IRepositoryFactory RepositoryFactory { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await using IDepartmentRepository repository = RepositoryFactory.BuildDepartmentRepository();

        Departments = await repository
            .FindPageAsync(1, int.MaxValue, false, true, false, default)
            .ToListAsync(default);

        Employee = new AddEmployeeDto() { DepartmentId = Departments.FirstOrDefault()?.Id ?? 0 };

    }

    private async Task OnValidSubmit()
    {
        if (Employee is null || IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            await using IEmployeeRepository repository = RepositoryFactory.BuildEmployeeRepository();
            await repository.AddEmployeeAsync(Employee, default);

            SuccessMessage = $"Successfully added {Employee.FirstName} {Employee.LastName}";
            ErrorMessage = null;

            Employee = new AddEmployeeDto() { DepartmentId = Employee.DepartmentId };

        }
        catch (Exception)
        {
            SuccessMessage = null;
            ErrorMessage = "An Error occurred adding the employee";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void OnInvalidSubmit()
    {
        SuccessMessage = null;
        ErrorMessage = null;
    }

    private Task OnSubmit(bool valid)
    {
        if (valid)
        {
            return OnValidSubmit();
        }
        else
        {
            OnInvalidSubmit();
            return Task.CompletedTask;
        }
    }
}

