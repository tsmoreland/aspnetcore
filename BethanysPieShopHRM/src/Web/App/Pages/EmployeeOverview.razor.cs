using BethanysPieShopHRM.Shared.Domain;
using BethanysPieShopHRM.Web.App.Services;
using Microsoft.AspNetCore.Components;

namespace BethanysPieShopHRM.Web.App.Pages;

public partial class EmployeeOverview
{
    private Employee? _selectedEmployee;

    public List<Employee>? Employees { get; set; }

    public string Title { get; set; } = "Employee Overview";

    public string SearchTerm { get; set; } = string.Empty;

    [Inject]
    public IEmployeeDataService EmployeeDataService { get; set; } = default!;

    /// <inheritdoc />
    protected override Task OnInitializedAsync()
    {
        return LoadEmployeesAsync();
    }

    public async Task LoadEmployeesAsync()
    {
        Employees = (await EmployeeDataService.GetAllEmployees().ConfigureAwait(false)).ToList();
    }

    public void ShowQuickViewPopup(Employee selectedEmployee)
    {
        _selectedEmployee = selectedEmployee;
    }

}
