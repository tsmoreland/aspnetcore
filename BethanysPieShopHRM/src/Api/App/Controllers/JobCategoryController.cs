using BethanysPieShopHRM.Api.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopHRM.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobCategoryController : Controller
{
    private readonly IJobCategoryRepository _jobCategoryRepository;

    public JobCategoryController(IJobCategoryRepository jobCategoryRepository)
    {
        _jobCategoryRepository = jobCategoryRepository;
    }


    [HttpGet]
    public IActionResult GetJobCategories()
    {
        return Ok(_jobCategoryRepository.GetAllJobCategories());
    }

    [HttpGet("{id}")]
    public IActionResult GetJobCategoryById(int id)
    {
        return Ok(_jobCategoryRepository.GetJobCategoryById(id));
    }
}
