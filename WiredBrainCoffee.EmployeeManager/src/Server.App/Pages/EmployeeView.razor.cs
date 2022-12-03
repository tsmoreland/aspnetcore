using Microsoft.AspNetCore.Components;
using WiredBrainCoffee.EmployeeManager.Domain.Contracts;
using WiredBrainCoffee.EmployeeManager.Domain.Models;
using WiredBrainCoffee.EmployeeManager.Shared;

namespace WiredBrainCoffee.EmployeeManager.Server.App.Pages;

public partial class EmployeeView
{
    [Parameter]
    public int EmployeeId { get; set; } 

    private Employee? Employee { get; set; }

    private bool IsBusy { get; set; }

    [Inject]
    private IRepositoryFactory RepositoryFactory { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private StateContainer SharedState { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        IsBusy = true;
        try
        {
            await using IEmployeeRepository employeeRepository = RepositoryFactory.BuildEmployeeRepository();

            Employee? employee = await employeeRepository.FindByIdAsync(EmployeeId, true, false, default);
            if (employee is null)
            {
                NavigationManager.NavigateTo($"/employees/list/{SharedState.EmployeeOverviewPage}");
                return;
            }

            Employee = employee;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
