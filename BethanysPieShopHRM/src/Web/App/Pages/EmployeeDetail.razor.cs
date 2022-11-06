﻿using BethanysPieShopHRM.Shared.Domain;
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

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        Employee = await EmployeeDataService.GetEmployeeDetails(int.Parse(EmployeeId));
    }
}
