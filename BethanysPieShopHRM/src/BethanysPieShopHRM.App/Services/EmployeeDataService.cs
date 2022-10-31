using System.Text.Json;
using BethanysPieShopHRM.Shared.Domain;
using BethanysPieShopHRM.App.Infrastructure;
using Blazored.LocalStorage;

namespace BethanysPieShopHRM.App.Services;

public sealed class EmployeeDataService : IEmployeeDataService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly JsonSerializerOptions _caseSensitiveOptions;

    public EmployeeDataService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
        _caseSensitiveOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Employee>> GetAllEmployees(bool refreshRequired = false)
    {
        if (!refreshRequired)
        {
            bool employeeExpirationExists = await _localStorage.ContainKeyAsync(LocalStorageContants.EmployeesListExpirationKey);
            if (employeeExpirationExists)
            {
                DateTime expiration = await _localStorage.GetItemAsync<DateTime>(LocalStorageContants.EmployeesListExpirationKey);
                if (expiration > DateTime.UtcNow && await _localStorage.ContainKeyAsync(LocalStorageContants.EmployeesListKey))
                {
                    return await _localStorage.GetItemAsync<List<Employee>>(LocalStorageContants.EmployeesListKey);
                }
            }
        }

        Stream response = await _httpClient.GetStreamAsync("api/employee");
        List<Employee> employees = (await JsonSerializer.DeserializeAsync<IEnumerable<Employee>>(response, _caseSensitiveOptions) ?? Array.Empty<Employee>()).ToList();

        await _localStorage.SetItemAsync(LocalStorageContants.EmployeesListKey, employees);
        await _localStorage.SetItemAsync(LocalStorageContants.EmployeesListExpirationKey, DateTime.UtcNow.AddMinutes(1));

        return employees;
    }

    /// <inheritdoc />
    public async Task<Employee?> GetEmployeeDetails(int employeeId)
    {
        return await JsonSerializer.DeserializeAsync<Employee>(await _httpClient.GetStreamAsync($"api/employee/{employeeId}"), _caseSensitiveOptions);
    }

    /// <inheritdoc />
    public Task<Employee> AddEmployee(Employee employee)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task UpdateEmployee(Employee employee)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task DeleteEmployee(int employeeId)
    {
        throw new NotImplementedException();
    }
}
