using BethanysPieShopHRM.Shared.Domain;
using BethanysPieShopHRM.Web.App.Services;
using Microsoft.AspNetCore.Components;

namespace BethanysPieShopHRM.Web.App.Pages;

public partial class EmployeeOverview
{
    private Employee? _selectedEmployee;

    public List<Employee>? Employees { get; set; } = default;

    public string Title { get; set; } = "Employee Overview";

    [Inject]
    public IEmployeeDataService EmployeeDataService { get; set; } = default!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        Employees = (await EmployeeDataService.GetAllEmployees()).ToList();
    }

    public void ShowQuickViewPopup(Employee selectedEmployee)
    {
        _selectedEmployee = selectedEmployee;
    }

}
