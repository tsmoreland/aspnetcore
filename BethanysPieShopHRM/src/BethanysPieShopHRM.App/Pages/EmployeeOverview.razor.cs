using BethanysPieShopHRM.App.Services;
using BethanysPieShopHRM.Shared.Domain;

namespace BethanysPieShopHRM.App.Pages;

public partial class EmployeeOverview
{
    public List<Employee>? Employees { get; set; } = default;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        Employees = MockDataService.Employees;
    }
}
