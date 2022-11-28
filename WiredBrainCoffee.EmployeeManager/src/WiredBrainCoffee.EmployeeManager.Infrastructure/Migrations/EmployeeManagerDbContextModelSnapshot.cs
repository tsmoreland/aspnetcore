﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WiredBrainCoffee.EmployeeManager.Infrastructure;

#nullable disable

namespace WiredBrainCoffee.EmployeeManager.Infrastructure.Migrations
{
    [DbContext(typeof(EmployeeManagerDbContext))]
    partial class EmployeeManagerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("WiredBrainCoffee.EmployeeManager.Infrastructure.Entities.DepartmentEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Departments");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Finance"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Sales"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Marketing"
                        },
                        new
                        {
                            Id = 4,
                            Name = "HR"
                        },
                        new
                        {
                            Id = 5,
                            Name = "IT"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Research and Development"
                        });
                });

            modelBuilder.Entity("WiredBrainCoffee.EmployeeManager.Infrastructure.Entities.EmployeeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeveloper")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DepartmentId = 1,
                            FirstName = "John",
                            IsDeveloper = false,
                            LastName = "Smith"
                        },
                        new
                        {
                            Id = 2,
                            DepartmentId = 2,
                            FirstName = "Jessica",
                            IsDeveloper = false,
                            LastName = "Jones"
                        },
                        new
                        {
                            Id = 3,
                            DepartmentId = 3,
                            FirstName = "Tony",
                            IsDeveloper = true,
                            LastName = "Stark"
                        },
                        new
                        {
                            Id = 4,
                            DepartmentId = 4,
                            FirstName = "Edward",
                            IsDeveloper = false,
                            LastName = "Enigma"
                        },
                        new
                        {
                            Id = 5,
                            DepartmentId = 5,
                            FirstName = "Brenda",
                            IsDeveloper = false,
                            LastName = "Moore"
                        },
                        new
                        {
                            Id = 6,
                            DepartmentId = 6,
                            FirstName = "Bruce",
                            IsDeveloper = true,
                            LastName = "Wayne"
                        });
                });

            modelBuilder.Entity("WiredBrainCoffee.EmployeeManager.Infrastructure.Entities.EmployeeEntity", b =>
                {
                    b.HasOne("WiredBrainCoffee.EmployeeManager.Infrastructure.Entities.DepartmentEntity", "Department")
                        .WithMany("Employees")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("WiredBrainCoffee.EmployeeManager.Infrastructure.Entities.DepartmentEntity", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
