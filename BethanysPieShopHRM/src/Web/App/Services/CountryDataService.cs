using System.Text.Json;
using BethanysPieShopHRM.Shared.Domain;

namespace BethanysPieShopHRM.Web.App.Services;

public sealed class CountryDataService : ICountryDataService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _caseSensitiveOptions;

    public CountryDataService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _caseSensitiveOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Country>> GetAllCountries()
    {
        Stream response = await _httpClient.GetStreamAsync("api/country");
        List<Country> countries = (await JsonSerializer.DeserializeAsync<IEnumerable<Country>>(response, _caseSensitiveOptions) ?? Array.Empty<Country>()).ToList();
        return countries;
    }

    /// <inheritdoc />
    public async Task<Country?> GetCountryuById(int countryId)
    {
        Stream response = await _httpClient.GetStreamAsync($"api/country/{countryId}");
        return await JsonSerializer.DeserializeAsync<Country>(response, _caseSensitiveOptions);
    }
}
