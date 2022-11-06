using System.Text.Json;
using BethanysPieShopHRM.Shared.Domain;

namespace BethanysPieShopHRM.App.Services;

public sealed class JobCategoryDataService : IJobCategoryDataService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _caseSensitiveOptions;

    public JobCategoryDataService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _caseSensitiveOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    /// <inheritdoc />
    public async Task<IEnumerable<JobCategory>> GetAllJobCatagories()
    {
        Stream response = await _httpClient.GetStreamAsync("api/jobcategory");
        List<JobCategory> catagories = (await JsonSerializer.DeserializeAsync<IEnumerable<JobCategory>>(response, _caseSensitiveOptions) ?? Array.Empty<JobCategory>()).ToList();
        return catagories;
    }

    /// <inheritdoc />
    public async Task<JobCategory?> GetJobCatagoryById(int jobCatagoryId)
    {
        return await JsonSerializer
            .DeserializeAsync<JobCategory>(await _httpClient.GetStreamAsync($"api/jobcategory/{jobCatagoryId}"), _caseSensitiveOptions);
    }
}
