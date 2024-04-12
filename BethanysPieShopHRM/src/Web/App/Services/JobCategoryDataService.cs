using System.Text.Json;
using BethanysPieShopHRM.Shared.Domain;

namespace BethanysPieShopHRM.Web.App.Services;

public sealed class JobCategoryDataService(HttpClient httpClient) : IJobCategoryDataService
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private readonly JsonSerializerOptions _caseSensitiveOptions = new() { PropertyNameCaseInsensitive = true };

    /// <inheritdoc />
    public async Task<IEnumerable<JobCategory>> GetAllJobCatagories()
    {
        Stream response = await _httpClient.GetStreamAsync("api/jobcategory");
        List<JobCategory> catagories = (await JsonSerializer.DeserializeAsync<IEnumerable<JobCategory>>(response, _caseSensitiveOptions) ?? []).ToList();
        return catagories;
    }

    /// <inheritdoc />
    public async Task<JobCategory?> GetJobCatagoryById(int jobCatagoryId)
    {
        return await JsonSerializer
            .DeserializeAsync<JobCategory>(await _httpClient.GetStreamAsync($"api/jobcategory/{jobCatagoryId}"), _caseSensitiveOptions);
    }
}
