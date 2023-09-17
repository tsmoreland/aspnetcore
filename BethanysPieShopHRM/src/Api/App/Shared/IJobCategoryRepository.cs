using BethanysPieShopHRM.Shared.Domain;

namespace BethanysPieShopHRM.Api.App.Shared;

public interface IJobCategoryRepository
{
    IEnumerable<JobCategory> GetAllJobCategories();
    JobCategory? GetJobCategoryById(int jobCategoryId);
}
