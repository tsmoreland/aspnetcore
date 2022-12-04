using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WiredBrainCoffee.EmployeeManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Concurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LastModifiedTime",
                table: "Employees",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "Employees",
                type: "integer",
                rowVersion: true,
                nullable: true,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "LastModifiedTime",
                table: "Departments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "Departments",
                type: "integer",
                rowVersion: true,
                nullable: true,
                defaultValue: 0L);

            migrationBuilder.Sql("""
                CREATE TRIGGER UpdateEmployeeVersion
                AFTER UPDATE ON Employees
                BEGIN
                    UPDATE Employees
                    SET Version = Version + 1
                    WHERE rowid = NEW.rowid;
                END;
                """);

            migrationBuilder.Sql("""
                CREATE TRIGGER UpdateDepartmentVersion
                AFTER UPDATE ON Departments
                BEGIN
                    UPDATE Departments
                    SET Version = Version + 1
                    WHERE rowid = NEW.rowid;
                END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModifiedTime",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LastModifiedTime",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Departments");

            migrationBuilder.Sql("""
                DROP TRIGGER UpdateEmployeeVersion
                """);

            migrationBuilder.Sql("""
                DROP TRIGGER UpdateDepartmentVersion
                """);
        }
    }
}
