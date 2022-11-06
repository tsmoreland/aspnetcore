﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BethanysPieShopHRM.Shared.Domain;

public class Employee
{
    public int EmployeeId { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "First name is too long")]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [StringLength(50, ErrorMessage = "Last name is too long")]
    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public Country? Country { get; set; } = default!;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool Smoker { get; set; }
    public MaritalStatus MaritalStatus { get; set; }
    public Gender Gender { get; set; }
    [StringLength(1000, ErrorMessage = "Comment length can't exceed 1000 characters.")]
    public string? Comment { get; set; }
    public DateTime? JoinedDate { get; set; }
    public DateTime? ExitDate { get; set; }

    public int JobCategoryId { get; set; }
    public JobCategory? JobCategory { get; set; } = default!;

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }


    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";

    [NotMapped]
    public byte[]? ImageContent { get; set; }

    public string? ImageName { get; set; }
}
