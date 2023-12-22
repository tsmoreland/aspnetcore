using BethanysPieShopHRM.Api.App.Shared;
using BethanysPieShopHRM.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShopHRM.Api.App.Infrastructure;

public class JobCategoryRepository(AppDbContext appDbContext) : IJobCategoryRepository
{
    public IEnumerable<JobCategory> GetAllJobCategories()
    {
        return appDbContext.JobCategories;
    }

    public JobCategory? GetJobCategoryById(int jobCategoryId)
    {
        return appDbContext.JobCategories
            .AsNoTracking()
            .FirstOrDefault(c => c.JobCategoryId == jobCategoryId);
    }
}
