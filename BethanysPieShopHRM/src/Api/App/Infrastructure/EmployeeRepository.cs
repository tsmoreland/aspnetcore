using BethanysPieShopHRM.Api.App.Shared;
using BethanysPieShopHRM.Shared.Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BethanysPieShopHRM.Api.App.Infrastructure;

public class EmployeeRepository(AppDbContext appDbContext) : IEmployeeRepository
{
    public IEnumerable<Employee> GetAllEmployees()
    {
        IEnumerable<Employee> employees = appDbContext.Employees.AsEnumerable();
        return employees;
    }

    public Employee? GetEmployeeById(int employeeId)
    {
        return appDbContext.Employees
            .FirstOrDefault(c => c.EmployeeId == employeeId);
    }

    public Employee AddEmployee(Employee employee)
    {
        EntityEntry<Employee> addedEntity = appDbContext.Employees.Add(employee);
        appDbContext.SaveChanges();
        return addedEntity.Entity;
    }

    public Employee? UpdateEmployee(Employee employee)
    {
        Employee? foundEmployee = appDbContext.Employees.Find(employee.EmployeeId);

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

        appDbContext.SaveChanges();

        return foundEmployee;

    }

    public void DeleteEmployee(int employeeId)
    {
        Employee? foundEmployee = appDbContext.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
        if (foundEmployee == null) return;

        appDbContext.Employees.Remove(foundEmployee);
        appDbContext.SaveChanges();
    }
}
