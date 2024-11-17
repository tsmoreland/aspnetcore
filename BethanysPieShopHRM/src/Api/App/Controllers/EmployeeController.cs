using BethanysPieShopHRM.Api.App.Shared;
using BethanysPieShopHRM.Shared.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopHRM.Api.App.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmployeeController(
    IEmployeeRepository employeeRepository,
    IWebHostEnvironment webHostEnvironment,
    IHttpContextAccessor httpContextAccessor)
    : Controller
{
    [HttpGet]
    public IActionResult GetAllEmployees()
    {
        return Ok(employeeRepository.GetAllEmployees());
    }

    [HttpGet("{id}")]
    public IActionResult GetEmployeeById(int id)
    {
        return Ok(employeeRepository.GetEmployeeById(id));
    }

    [HttpPost]
    public IActionResult CreateEmployee([FromBody] Employee employee)
    {
        if (employee == null!)
        {
            return BadRequest();
        }

        if (employee.FirstName == string.Empty || employee.LastName == string.Empty)
        {
            ModelState.AddModelError("Name/FirstName", "The name or first name shouldn't be empty");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // handle image upload
        if (employee.ImageContent is not null)
        {
            var context = httpContextAccessor.HttpContext;
            if (context is null)
            {
                return new ObjectResult("Internal service error - context not found") { StatusCode = StatusCodes.Status500InternalServerError };
            }

            var currentUrl = context.Request.Host.Value;
            var imageName = employee.ImageName?.Replace("..", string.Empty).Trim() ?? string.Empty;
            if (imageName is { Length: > 0 })
            {
                var path = Path.GetFullPath($"{webHostEnvironment.WebRootPath}\\uploads\\{employee.ImageName}");
                if (!path.StartsWith(webHostEnvironment.WebRootPath))
                {
                    return BadRequest();
                }

                using (var fileStream = System.IO.File.Create(path))
                {
                    fileStream.Write(employee.ImageContent, 0, employee.ImageContent.Length);
                    fileStream.Close();
                }

                employee.ImageName = $"https://{currentUrl}/uploads/{employee.ImageName}";
            }
            else
            {
                employee.ImageName = null;
            }
        }

        var createdEmployee = employeeRepository.AddEmployee(employee);

        return Created("employee", createdEmployee);
    }

    [HttpPut]
    public IActionResult UpdateEmployee([FromBody] Employee employee)
    {
        if (employee == null!)
        {
            return BadRequest();
        }

        if (employee.FirstName == string.Empty || employee.LastName == string.Empty)
        {
            ModelState.AddModelError("Name/FirstName", "The name or first name shouldn't be empty");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var employeeToUpdate = employeeRepository.GetEmployeeById(employee.EmployeeId);

        if (employeeToUpdate == null)
        {
            return NotFound();
        }

        employeeRepository.UpdateEmployee(employee);

        return NoContent(); //success
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteEmployee(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        var employeeToDelete = employeeRepository.GetEmployeeById(id);
        if (employeeToDelete == null)
        {
            return NotFound();
        }

        employeeRepository.DeleteEmployee(id);

        return NoContent();
    }
}
