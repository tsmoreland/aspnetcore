namespace WiredBrainCoffee.EmployeeManager.Domain.Models;

public sealed class Employee
{
    private Department _department;
    private string _firstName;
    private string _lastName;

    public Employee(int id, string firstName, string lastName, Department department)
        : this(firstName, lastName, department)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "id cannot be 0");
        }

        Id = id;
    }

    public Employee(string firstName, string lastName, Department department)
    {
        Id = 0;

        if (firstName is not {Length: > 0 and <= 100})
        {
            throw new ArgumentException("Name cannot be empty or more than 100 characters in length");
        }
        _firstName = firstName;

        if (lastName is not {Length: > 0 and <= 100})
        {
            throw new ArgumentException("Name cannot be empty or more than 100 characters in length");
        }
        _lastName = lastName;
        _department = department ?? throw new ArgumentNullException(nameof(department));
    }

    public int Id { get; }


    public string FirstName
    {
        get => _firstName;
        set
        {
            if (value is not {Length: > 0 and <= 100})
            {
                throw new ArgumentException("Name cannot be empty or more than 100 characters in length");
            }
            _firstName = value;
        }
    }
    public string LastName 
    {
        get => _lastName;
        set
        {
            if (value is not {Length: > 0 and <= 100})
            {
                throw new ArgumentException("Name cannot be empty or more than 100 characters in length");
            }
            _lastName = value;
        }
    }

    public Department Department
    {
        get => _department;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _department = value;
        }
    }
}
