using BethanysPieShopHRM.Shared.Domain;
using Microsoft.AspNetCore.Components;

namespace BethanysPieShopHRM.Web.App.Components;

public partial class EmployeeCard
{

    [Parameter]
    public Employee Employee { get; set; } = default!;

    [Parameter]
    public EventCallback<Employee> EmployeeQuickViewCLicked { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    public void NavigateToDetails(Employee selected)
    {
        NavigationManager.NavigateTo($"/employeedetail/{selected.EmployeeId}");
    }
}
