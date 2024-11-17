using BethanysPieShopHRM.Shared.Domain;
using BethanysPieShopHRM.Web.App.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BethanysPieShopHRM.Web.App.Pages;

public partial class EmployeeEdit
{
    private IBrowserFile? _selectedFile;

    [Inject]
    public IEmployeeDataService EmployeeDataService { get; set; } = default!;

    [Inject]
    public ICountryDataService CountryDataService { get; set; } = default!;

    [Inject]
    public IJobCategoryDataService JobCategoryDataService { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Parameter]
    public string? EmployeeId { get; set; }

    public Employee Employee { get; set; } = new();

    public List<Country> Countries { get; set; } = [];
    public List<JobCategory> JobCategories { get; set; } = [];

    protected string Message { get; set; } = string.Empty;
    protected string StatusClass { get; set; } = string.Empty;
    protected bool Saved { get; set; }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        Saved = false;
        var countryTask = CountryDataService.GetAllCountries().ConfigureAwait(false);
        var jobCategoryTask = JobCategoryDataService.GetAllJobCatagories().ConfigureAwait(false);

        Countries = (await countryTask).ToList();
        JobCategories = (await jobCategoryTask).ToList();

        if (int.TryParse(EmployeeId, out var employeeId))
        {
            var existing = await EmployeeDataService.GetEmployeeDetails(employeeId).ConfigureAwait(false);

            if (existing is not null)
            {
                Employee = existing;
            }
            else
            {
                // 404 not found, should show error page
            }
        }
        else
        {
            Employee = new Employee { CountryId = 10, JobCategoryId = 1, BirthDate = DateTime.Now, JoinedDate = DateTime.Now };
        }
    }

    private Task OnValidSubmit()
    {
        Saved = false;
        return Employee.EmployeeId == 0
           ? AddAsync()
           : UpdateAsync();
    }

    private async Task AddAsync()
    {
        if (_selectedFile is not null)
        {
            var file = _selectedFile;
            using MemoryStream memory = new();
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA2007 // can't use ConfigureAwait, or we won't be able to access the stream
            await using (var fileStream = file.OpenReadStream())
#pragma warning restore CA2007
            {
                await fileStream.CopyToAsync(memory).ConfigureAwait(false);
            }
#pragma warning restore IDE0079 // Remove unnecessary suppression

            Employee.ImageName = file.Name;
            Employee.ImageContent = memory.ToArray();
        }

        var added = await EmployeeDataService.AddEmployee(Employee).ConfigureAwait(false);
        if (added is not null)
        {
            StatusClass = "alert-success";
            Message = "New employee saved successfully";
            Saved = true;
        }
        else
        {
            StatusClass = "alert-danger";
            Message = "Something went wrong adding the new employee.  Please try again";
        }
    }
    private async Task UpdateAsync()
    {
        await EmployeeDataService.UpdateEmployee(Employee).ConfigureAwait(false);
        StatusClass = "alert-success";
        Message = "Employee updated successfully";
        Saved = true;
    }

    private void OnInvalidSubmit()
    {
        StatusClass = "alert-danger";
        Message = "There are some validation errors.  Please try again.";
    }

    private async Task OnDeleteClicked()
    {
        await EmployeeDataService.DeleteEmployee(Employee.EmployeeId).ConfigureAwait(false);
        StatusClass = "alert-success";
        Message = "Deleted Successfully";
        Saved = true;
    }

    private void NavigateToOverview()
    {
        NavigationManager.NavigateTo("/employeeoverview");
    }

    private void OnInputFileChange(InputFileChangeEventArgs e)
    {
        _selectedFile = e.File;
        StateHasChanged();
    }
}
