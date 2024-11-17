using System.Net.Http.Json;
using System.Text.Json;
using BethanysPieShopHRM.Shared.Domain;
using BethanysPieShopHRM.Web.App.Infrastructure;
using Blazored.LocalStorage;

namespace BethanysPieShopHRM.Web.App.Services;

public sealed class EmployeeDataService(HttpClient httpClient, ILocalStorageService localStorage) : IEmployeeDataService
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private readonly ILocalStorageService _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
    private readonly JsonSerializerOptions _caseSensitiveOptions = new() { PropertyNameCaseInsensitive = true };

    /// <inheritdoc />
    public async Task<IEnumerable<Employee>> GetAllEmployees(bool refreshRequired = false)
    {
        if (!refreshRequired)
        {
            var employeeExpirationExists = await _localStorage.ContainKeyAsync(LocalStorageContants.EmployeesListExpirationKey).ConfigureAwait(false);
            if (employeeExpirationExists)
            {
                var expiration = await _localStorage.GetItemAsync<DateTime>(LocalStorageContants.EmployeesListExpirationKey).ConfigureAwait(false);
                if (expiration > DateTime.UtcNow && await _localStorage.ContainKeyAsync(LocalStorageContants.EmployeesListKey).ConfigureAwait(false))
                {
                    return await _localStorage.GetItemAsync<List<Employee>>(LocalStorageContants.EmployeesListKey).ConfigureAwait(false) ?? [];
                }
            }
        }

        var response = await _httpClient.GetStreamAsync("api/employee").ConfigureAwait(false);
        var employees = (await JsonSerializer.DeserializeAsync<IEnumerable<Employee>>(response, _caseSensitiveOptions).ConfigureAwait(false) ?? []).ToList();

        await _localStorage.SetItemAsync(LocalStorageContants.EmployeesListKey, employees).ConfigureAwait(false);
        await _localStorage.SetItemAsync(LocalStorageContants.EmployeesListExpirationKey, DateTime.UtcNow.AddMinutes(1)).ConfigureAwait(false);

        return employees;
    }

    /// <inheritdoc />
    public async Task<Employee?> GetEmployeeDetails(int employeeId)
    {
        return await JsonSerializer.DeserializeAsync<Employee>(
            await _httpClient.GetStreamAsync($"api/employee/{employeeId}").ConfigureAwait(false), _caseSensitiveOptions)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<Employee?> AddEmployee(Employee employee)
    {
        var response = await _httpClient.PostAsJsonAsync<Employee>("api/employee", employee, default).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        var addedEmployee = await JsonSerializer.DeserializeAsync<Employee>(await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            .ConfigureAwait(false);
        if (addedEmployee is null)
        {
            return null;
        }

        if (await _localStorage.ContainKeyAsync(LocalStorageContants.EmployeesListExpirationKey).ConfigureAwait(false))
        {
            await _localStorage.RemoveItemAsync(LocalStorageContants.EmployeesListExpirationKey).ConfigureAwait(false);
        }

        return addedEmployee;
    }

    /// <inheritdoc />
    public async Task UpdateEmployee(Employee employee)
    {
        var response = await _httpClient.PutAsJsonAsync<Employee>($"api/employee/{employee.EmployeeId}", employee, default).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            // log error... and ideally return something that represents an error - even if that means throwing an exception
        }
    }

    /// <inheritdoc />
    public async Task DeleteEmployee(int employeeId)
    {
        var response = await _httpClient.DeleteAsync($"api/employee/{employeeId}", default).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            // log error... and ideally return something that represents an error - even if that means throwing an exception
        }
    }
}
