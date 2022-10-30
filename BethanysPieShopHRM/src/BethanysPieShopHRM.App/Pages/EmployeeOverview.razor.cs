using BethanysPieShopHRM.App.Services;
using BethanysPieShopHRM.Shared.Domain;

namespace BethanysPieShopHRM.App.Pages;

public partial class EmployeeOverview
{
    private Employee? _selectedEmployee;

    public List<Employee>? Employees { get; set; } = default;
    private string Title { get; set; } = "Employee Overview";

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        Employees = MockDataService.Employees;
    }

    public void ShowQuickViewPopup(Employee selectedEmployee)
    {
        _selectedEmployee = selectedEmployee;
    }

}
