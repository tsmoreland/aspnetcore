using System.Globalization;
using BethanysPieShopHRM.Shared.Domain;
using BethanysPieShopHRM.Shared.Models;
using BethanysPieShopHRM.Web.App.Services;
using Microsoft.AspNetCore.Components;

namespace BethanysPieShopHRM.Web.App.Pages;

public partial class EmployeeDetail
{
    [Parameter]
    public string EmployeeId { get; set; } = default!;

    public Employee? Employee { get; set; } = new();

    [Inject]
    public IEmployeeDataService EmployeeDataService { get; set; } = default!;

    public List<Marker> MapMarkers { get; set; } = default!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        Employee = await EmployeeDataService.GetEmployeeDetails(int.Parse(EmployeeId, CultureInfo.InvariantCulture)).ConfigureAwait(false);
        if (Employee is { Longitude: not null, Latitude: not null })
        {
            MapMarkers =
            [
                new Marker(Employee.FullName, Employee.Longitude.Value, Employee.Latitude.Value, false),
            ];
        }
    }
}
