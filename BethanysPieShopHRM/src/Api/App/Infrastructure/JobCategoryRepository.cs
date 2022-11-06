using BethanysPieShopHRM.Api.App.Shared;
using BethanysPieShopHRM.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShopHRM.Api.App.Infrastructure;

public class JobCategoryRepository: IJobCategoryRepository
{
    private readonly AppDbContext _appDbContext;

    public JobCategoryRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IEnumerable<JobCategory> GetAllJobCategories()
    {
        return _appDbContext.JobCategories;
    }

    public JobCategory? GetJobCategoryById(int jobCategoryId)
    {
        return _appDbContext.JobCategories
            .AsNoTracking()
            .FirstOrDefault(c => c.JobCategoryId == jobCategoryId);
    }
}
