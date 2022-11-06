using BethanysPieShopHRM.Api.App.Shared;
using BethanysPieShopHRM.Shared.Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BethanysPieShopHRM.Api.App.Infrastructure;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _appDbContext;

    public EmployeeRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IEnumerable<Employee> GetAllEmployees()
    {
        IEnumerable<Employee> employees = _appDbContext.Employees.AsEnumerable();
        return employees;
    }

    public Employee? GetEmployeeById(int employeeId)
    {
        return _appDbContext.Employees
            .FirstOrDefault(c => c.EmployeeId == employeeId);
    }

    public Employee AddEmployee(Employee employee)
    {
        EntityEntry<Employee> addedEntity = _appDbContext.Employees.Add(employee);
        _appDbContext.SaveChanges();
        return addedEntity.Entity;
    }

    public Employee? UpdateEmployee(Employee employee)
    {
        Employee? foundEmployee = _appDbContext.Employees.Find(employee.EmployeeId);

        if (foundEmployee == null)
        {
            return null;
        }

        foundEmployee.CountryId = employee.CountryId;
        foundEmployee.MaritalStatus = employee.MaritalStatus;
        foundEmployee.BirthDate = employee.BirthDate;
        foundEmployee.City = employee.City;
        foundEmployee.Email = employee.Email;
        foundEmployee.FirstName = employee.FirstName;
        foundEmployee.LastName = employee.LastName;
        foundEmployee.Gender = employee.Gender;
        foundEmployee.PhoneNumber = employee.PhoneNumber;
        foundEmployee.Smoker = employee.Smoker;
        foundEmployee.Street = employee.Street;
        foundEmployee.Zip = employee.Zip;
        foundEmployee.JobCategoryId = employee.JobCategoryId;
        foundEmployee.Comment = employee.Comment;
        foundEmployee.ExitDate = employee.ExitDate;
        foundEmployee.JoinedDate = employee.JoinedDate;
        //foundEmployee.ImageContent = employee.ImageContent;
        //foundEmployee.ImageName = employee.ImageName;

        _appDbContext.SaveChanges();

        return foundEmployee;

    }

    public void DeleteEmployee(int employeeId)
    {
        var foundEmployee = _appDbContext.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
        if (foundEmployee == null) return;

        _appDbContext.Employees.Remove(foundEmployee);
        _appDbContext.SaveChanges();
    }
}
