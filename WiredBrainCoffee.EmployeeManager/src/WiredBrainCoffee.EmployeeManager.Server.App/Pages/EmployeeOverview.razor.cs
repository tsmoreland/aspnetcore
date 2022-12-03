using Microsoft.AspNetCore.Components;
using WiredBrainCoffee.EmployeeManager.Domain.Contracts;
using WiredBrainCoffee.EmployeeManager.Domain.Models;
using WiredBrainCoffee.EmployeeManager.Infrastructure.Contracts;
using WiredBrainCoffee.EmployeeManager.Server.App.Shared;

namespace WiredBrainCoffee.EmployeeManager.Server.App.Pages;

public partial class EmployeeOverview
{
    private IReadOnlyList<Employee>? Employees { get; set; }

    private const int PageSize = 4;

    [Parameter]
    public int? CurrentPage { get; set; }

    public int TotalPages { get; private set; } = 1;

    [Inject]
    private IRepositoryFactory RepositoryFactory { get; set; } = null!;

    [Inject]
    private NavigationManager Navigation { get; set; } = null!;
    
    [Inject]
    private StateContainer SharedState { get; set; } = null!;


    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        if (CurrentPage is null or < 1)
        {
            Navigation.NavigateTo("/employees/list/1");
            return;
        }

        await using IEmployeeRepository repository = RepositoryFactory.BuildEmployeeRepository();
        int totalCount = await repository.GetTotalCount(default);
        TotalPages = totalCount != 0
            ? (int)Math.Ceiling((double)totalCount / PageSize)
            : 1;

        if (CurrentPage > TotalPages)
        {
            Navigation.NavigateTo($"/employees/list/{TotalPages}");
            return;
        }

        SharedState.EmployeeOverviewPage = CurrentPage.Value;

        List<Employee> employees = await repository.FindPageAsync(CurrentPage.Value, PageSize, true, false, default).ToListAsync(default);
        Employees = employees.AsReadOnly();
    }
}
