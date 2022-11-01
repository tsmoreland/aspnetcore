using BethanysPieShopHRM.App.Services;
using BethanysPieShopHRM.Shared.Domain;
using Microsoft.AspNetCore.Components;

namespace BethanysPieShopHRM.App.Pages;

public partial class EmployeeEdit
{
    [Inject]
    public IEmployeeDataService EmployeeDataService { get; set; } = default!;

    [Inject]
    public ICountryDataService CountryDataService { get; set; } = default!;

    [Inject]
    public IJobCategoryDataService JobCategoryDataService { get; set; } = default!;

    [Parameter]
    public string? EmployeeId { get; set; }

    public Employee Employee { get; set; } = new();

    public List<Country> Countries { get; set; } = new();
    public List<JobCategory> JobCategories { get; set; } = new();

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
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
}
