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

    public List<Country> Countries { get; set; } = new();
    public List<JobCategory> JobCategories { get; set; } = new();

    protected string Message { get; set; } = string.Empty;
    protected string StatusClass { get; set; } = string.Empty;
    protected bool Saved { get; set; }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        Saved = false;
        Task<IEnumerable<Country>> countryTask = CountryDataService.GetAllCountries();
        Task<IEnumerable<JobCategory>> jobCategoryTask = JobCategoryDataService.GetAllJobCatagories();

        Countries = (await countryTask).ToList();
        JobCategories = (await jobCategoryTask).ToList();

        if (int.TryParse(EmployeeId, out int employeeId))
        {
            Employee? existing = await EmployeeDataService.GetEmployeeDetails(employeeId);

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
            IBrowserFile file = _selectedFile;
            using MemoryStream memory = new();
            await using (Stream fileStream = file.OpenReadStream())
            {
                await fileStream.CopyToAsync(memory);
            }

            Employee.ImageName = file.Name;
            Employee.ImageContent = memory.ToArray();
        }

        Employee? added = await EmployeeDataService.AddEmployee(Employee);
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
        await EmployeeDataService.UpdateEmployee(Employee);
        StatusClass = "alert-success";
        Message = "Employee updated successfully";
        Saved = true;
    }

    private void OnInvalidSubmit()
    {
        StatusClass = "alert-danger";
        Message = "Thear are some validation errors.  Please try again.";
    }

    private async Task OnDeleteClicked()
    {
        await EmployeeDataService.DeleteEmployee(Employee.EmployeeId);
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
