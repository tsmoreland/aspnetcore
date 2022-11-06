using BethanysPieShopHRM.App.Services;
using BethanysPieShopHRM.Shared.Domain;
using Microsoft.AspNetCore.Components;

namespace BethanysPieShopHRM.App.Pages;

public partial class EmployeeDetail
{
    [Parameter]
    public string EmployeeId { get; set; } = default!;

    public Employee? Employee { get; set; } = new();

    [Inject]
    public IEmployeeDataService EmployeeDataService { get; set; } = default!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        Employee = await EmployeeDataService.GetEmployeeDetails(int.Parse(EmployeeId));
    }
}
