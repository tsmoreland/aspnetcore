using System.Text.Json;
using BethanysPieShopHRM.Shared.Domain;

namespace BethanysPieShopHRM.Web.App.Services;

public sealed class CountryDataService(HttpClient httpClient) : ICountryDataService
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private readonly JsonSerializerOptions _caseSensitiveOptions = new() { PropertyNameCaseInsensitive = true };

    /// <inheritdoc />
    public async Task<IEnumerable<Country>> GetAllCountries()
    {
        Stream response = await _httpClient.GetStreamAsync("api/country");
        List<Country> countries = (await JsonSerializer.DeserializeAsync<IEnumerable<Country>>(response, _caseSensitiveOptions) ?? []).ToList();
        return countries;
    }

    /// <inheritdoc />
    public async Task<Country?> GetCountryuById(int countryId)
    {
        Stream response = await _httpClient.GetStreamAsync($"api/country/{countryId}");
        return await JsonSerializer.DeserializeAsync<Country>(response, _caseSensitiveOptions);
    }
}
