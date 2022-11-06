
using BethanysPieShopHRM.Shared.Domain;

namespace BethanysPieShopHRM.App.Services;

public sealed class MockDataService
{
    private static List<Employee>? s_employees = default!;
    private static List<JobCategory> s_jobCategories = default!;
    private static List<Country> s_countries = default!;

    public static List<Employee> Employees
    {
        get
        {
            //we will use these in initialization of Employees
            s_countries ??= InitializeMockCountries();

            s_jobCategories ??= InitializeMockJobCategories();

            s_employees ??= InitializeMockEmployees();

            return s_employees;
        }
    }

    private static List<Employee> InitializeMockEmployees()
    {
        var bethany = new Employee
        {
            MaritalStatus = MaritalStatus.Single,
            BirthDate = new DateTime(1989, 3, 11),
            City = "Brussels",
            Email = "bethany@bethanyspieshop.com",
            EmployeeId = 1,
            FirstName = "Bethany",
            LastName = "Smith",
            Gender = Gender.Female,
            PhoneNumber = "324777888773",
            Smoker = false,
            Street = "Grote Markt 1",
            Zip = "1000",
            JobCategory = s_jobCategories[2],
            JobCategoryId = s_jobCategories[2].JobCategoryId,
            Comment = "Lorem Ipsum",
            ExitDate = null,
            JoinedDate = new DateTime(2015, 3, 1),
            Country = s_countries[0],
            CountryId = s_countries[0].CountryId
        };

        var gill = new Employee
        {
            MaritalStatus = MaritalStatus.Married,
            BirthDate = new DateTime(1979, 1, 16),
            City = "Antwerp",
            Email = "john@bethanyspieshop.com",
            EmployeeId = 2,
            FirstName = "Gill",
            LastName = "Cleeren",
            Gender = Gender.Male,
            PhoneNumber = "33999909923",
            Smoker = false,
            Street = "New Street",
            Zip = "2000",
            JobCategory = s_jobCategories[1],
            JobCategoryId = s_jobCategories[1].JobCategoryId,
            Comment = "Lorem Ipsum",
            ExitDate = null,
            JoinedDate = new DateTime(2017, 12, 24),
            Country = s_countries[1],
            CountryId = s_countries[1].CountryId
        };

        return new List<Employee> { bethany, gill };
    }

    private static List<JobCategory> InitializeMockJobCategories()
    {
        return new List<JobCategory>
        {
            new() {JobCategoryId = 1, JobCategoryName = "Pie research"},
            new() {JobCategoryId = 2, JobCategoryName = "Sales"},
            new() {JobCategoryId = 3, JobCategoryName = "Management"},
            new() {JobCategoryId = 4, JobCategoryName = "Store staff"},
            new() {JobCategoryId = 5, JobCategoryName = "Finance"},
            new() {JobCategoryId = 6, JobCategoryName = "QA"},
            new() {JobCategoryId = 7, JobCategoryName = "IT"},
            new() {JobCategoryId = 8, JobCategoryName = "Cleaning"},
            new() {JobCategoryId = 9, JobCategoryName = "Bakery"},
        };
    }

    private static List<Country> InitializeMockCountries()
    {
        return new List<Country>
        {
            new() {CountryId = 1, Name = "Belgium"},
            new() {CountryId = 2, Name = "Netherlands"},
            new() {CountryId = 3, Name = "USA"},
            new() {CountryId = 4, Name = "Japan"},
            new() {CountryId = 5, Name = "China"},
            new() {CountryId = 6, Name = "UK"},
            new() {CountryId = 7, Name = "France"},
            new() {CountryId = 8, Name = "Brazil"},
            new() {CountryId = 9, Name = "Canada"},
        };
    }
}
