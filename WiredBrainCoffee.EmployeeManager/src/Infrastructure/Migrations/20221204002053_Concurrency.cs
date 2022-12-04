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
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyToken",
                table: "Employees",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "LastModifiedTime",
                table: "Employees",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyToken",
                table: "Departments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "LastModifiedTime",
                table: "Departments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyToken", "LastModifiedTime" },
                values: new object[] { "160be889-f4f0-464e-a14e-6bb13686c44a", 0L });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyToken", "LastModifiedTime" },
                values: new object[] { "4e6a2a71-1dba-41b1-99c1-de4832942cff", 0L });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyToken", "LastModifiedTime" },
                values: new object[] { "e6846c79-bbcd-44bd-87ca-6bd219956bdf", 0L });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyToken", "LastModifiedTime" },
                values: new object[] { "f5aa945f-9a3c-4fc2-a2ff-7d2c2299ca69", 0L });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyToken", "LastModifiedTime" },
                values: new object[] { "fa002166-9df3-4221-94c7-27ed8df4c188", 0L });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyToken", "LastModifiedTime" },
                values: new object[] { "7a2e4c49-83fe-4f55-8340-002618c42183", 0L });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyToken", "LastModifiedTime" },
                values: new object[] { "2e0270ee-a83b-4cdf-ae89-8057fd68b972", 0L });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyToken", "LastModifiedTime" },
                values: new object[] { "8b65cdfe-d1e3-4029-83bd-25262d129ead", 0L });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyToken", "LastModifiedTime" },
                values: new object[] { "1d4edfae-1ec1-4d32-9f21-77834035a8d6", 0L });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyToken", "LastModifiedTime" },
                values: new object[] { "263cf54c-4e7a-400b-b216-4e3a4b34800d", 0L });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyToken", "LastModifiedTime" },
                values: new object[] { "c7d62fda-e896-450f-a572-701b1d69eb67", 0L });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyToken", "LastModifiedTime" },
                values: new object[] { "ed4ddc84-cdb6-42d3-a4fb-ef3c24a1e2e4", 0L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyToken",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LastModifiedTime",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ConcurrencyToken",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "LastModifiedTime",
                table: "Departments");
        }
    }
}
