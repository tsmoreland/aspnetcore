using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BethanysPieShopHRM.Api.Migrations
{
    public partial class EmployeeImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Employees",
                type: "TEXT",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryId", "Name" },
                values: new object[] { 10, "Canada" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: 10);

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Employees");
        }
    }
}
