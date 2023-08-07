using BethanysPieShopHRM.Shared.Domain;

namespace BethanysPieShopHRM.Web.App.Services;

public interface IJobCategoryDataService
{
    Task<IEnumerable<JobCategory>> GetAllJobCatagories();
    Task<JobCategory?> GetJobCatagoryById(int jobCatagoryId);
}
