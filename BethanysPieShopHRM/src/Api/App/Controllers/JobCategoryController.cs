using BethanysPieShopHRM.Api.App.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopHRM.Api.App.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobCategoryController(IJobCategoryRepository jobCategoryRepository) : Controller
{
    [HttpGet]
    public IActionResult GetJobCategories()
    {
        return Ok(jobCategoryRepository.GetAllJobCategories());
    }

    [HttpGet("{id}")]
    public IActionResult GetJobCategoryById(int id)
    {
        return Ok(jobCategoryRepository.GetJobCategoryById(id));
    }
}
