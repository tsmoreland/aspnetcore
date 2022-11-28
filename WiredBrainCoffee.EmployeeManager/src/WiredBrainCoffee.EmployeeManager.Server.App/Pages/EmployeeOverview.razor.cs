using Microsoft.AspNetCore.Components;
using WiredBrainCoffee.EmployeeManager.Domain.Contracts;
using WiredBrainCoffee.EmployeeManager.Domain.Models;

namespace WiredBrainCoffee.EmployeeManager.Server.App.Pages;

public partial class EmployeeOverview
{
    private IReadOnlyList<Employee>? Employees { get; set; }

    [Inject]
    private IRepositoryFactory RepositoryFactory { get; set; } = null!;


    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await using IEmployeeRepository repository = RepositoryFactory.BuildEmployeeRepository();
        List<Employee> employees = await repository.FindAllAsync(default).ToListAsync(default);
        Employees = employees.AsReadOnly();
    }
}
