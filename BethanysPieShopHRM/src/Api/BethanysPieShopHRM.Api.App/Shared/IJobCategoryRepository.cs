using BethanysPieShopHRM.Shared.Domain;

namespace BethanysPieShopHRM.Api.Shared;

public interface IJobCategoryRepository
{
    IEnumerable<JobCategory> GetAllJobCategories();
    JobCategory? GetJobCategoryById(int jobCategoryId);
}
