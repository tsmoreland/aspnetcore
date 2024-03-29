﻿namespace WiredBrainCoffee.EmployeeManager.Domain.Models;

public sealed class Employee
{
    private Department _department;
    private string _firstName;
    private string _lastName;

    public Employee(int id, string firstName, string lastName, bool isDeveloper, Department department)
        : this(firstName, lastName, isDeveloper, department)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "id cannot be 0");
        }

        Id = id;
    }

    public Employee(int id, string firstName, string lastName, bool isDeveloper, Department department, object? dataBacking)
        : this(id, firstName, lastName, isDeveloper, department)
    {
        Databacking = dataBacking;
    }

    public Employee(string firstName, string lastName, bool isDeveloper, Department department)
    {
        Id = 0;

        if (firstName is not { Length: > 0 and <= 100 })
        {
            throw new ArgumentException("Name cannot be empty or more than 100 characters in length", nameof(firstName));
        }
        _firstName = firstName;

        if (lastName is not { Length: > 0 and <= 100 })
        {
            throw new ArgumentException("Name cannot be empty or more than 100 characters in length", nameof(lastName));
        }
        _lastName = lastName;
        _department = department ?? throw new ArgumentNullException(nameof(department));
        IsDeveloper = isDeveloper;
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

    public bool IsDeveloper { get; set; }

    /// <summary>
    /// Underlying entity used to store the object
    /// </summary>
    public object? Databacking { get; init; }

}
