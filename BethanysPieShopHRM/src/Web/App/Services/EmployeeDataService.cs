using System.Net.Http.Json;
using System.Text.Json;
using BethanysPieShopHRM.Shared.Domain;
using BethanysPieShopHRM.Web.App.Infrastructure;
using Blazored.LocalStorage;

namespace BethanysPieShopHRM.Web.App.Services;

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
    public async Task<Employee?> AddEmployee(Employee employee)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Employee>("api/employee", employee, default);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        Employee? addedEmployee = await JsonSerializer.DeserializeAsync<Employee>(await response.Content.ReadAsStreamAsync());
        if (addedEmployee is null)
        {
            return null;
        }

        if (await _localStorage.ContainKeyAsync(LocalStorageContants.EmployeesListExpirationKey))
        {
            await _localStorage.RemoveItemAsync(LocalStorageContants.EmployeesListExpirationKey);
        }

        return addedEmployee;
    }

    /// <inheritdoc />
    public async Task UpdateEmployee(Employee employee)
    {
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync<Employee>($"api/employee/{employee.EmployeeId}", employee, default);
        if (!response.IsSuccessStatusCode)
        {
            // log error... and ideally return something that represents an error - even if that means throwing an exception
        }
    }

    /// <inheritdoc />
    public async Task DeleteEmployee(int employeeId)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"api/employee/{employeeId}", default);
        if (!response.IsSuccessStatusCode)
        {
            // log error... and ideally return something that represents an error - even if that means throwing an exception
        }
    }
}
