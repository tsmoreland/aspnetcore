using BethanysPieShopHRM.Api.App.Shared;
using BethanysPieShopHRM.Shared.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopHRM.Api.App.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : Controller
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EmployeeController(IEmployeeRepository employeeRepository, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
    {
        _employeeRepository = employeeRepository;
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public IActionResult GetAllEmployees()
    {
        return Ok(_employeeRepository.GetAllEmployees());
    }

    [HttpGet("{id}")]
    public IActionResult GetEmployeeById(int id)
    {
        return Ok(_employeeRepository.GetEmployeeById(id));
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
            HttpContext? context = _httpContextAccessor.HttpContext;
            if (context is null)
            {
                return new ObjectResult("Internal service error - context not found") { StatusCode = StatusCodes.Status500InternalServerError };
            }

            string currentUrl = context.Request.Host.Value;
            string path = $"{_webHostEnvironment.WebRootPath}\\uploads\\{employee.ImageName}";
            using (FileStream fileStream = System.IO.File.Create(path))
            {
                fileStream.Write(employee.ImageContent, 0, employee.ImageContent.Length);
                fileStream.Close();
            }

            employee.ImageName = $"https://{currentUrl}/uploads/{employee.ImageName}";
        }


        Employee createdEmployee = _employeeRepository.AddEmployee(employee);

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

        Employee? employeeToUpdate = _employeeRepository.GetEmployeeById(employee.EmployeeId);

        if (employeeToUpdate == null)
        {
            return NotFound();
        }

        _employeeRepository.UpdateEmployee(employee);

        return NoContent(); //success
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteEmployee(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        Employee? employeeToDelete = _employeeRepository.GetEmployeeById(id);
        if (employeeToDelete == null)
        {
            return NotFound();
        }

        _employeeRepository.DeleteEmployee(id);

        return NoContent();
    }
}
