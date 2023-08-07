namespace WiredBrainCoffee.EmployeeManager.Infrastructure.Entities;

public sealed class EmployeeEntity : Entity<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsDeveloper { get; set; }

    public int DepartmentId { get; set; }
    public DepartmentEntity? Department { get; set; }
}
