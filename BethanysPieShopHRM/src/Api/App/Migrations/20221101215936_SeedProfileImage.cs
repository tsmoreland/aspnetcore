using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BethanysPieShopHRM.Api.Migrations
{
    public partial class SeedProfileImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "ImageName",
                value: "/uploads/526DF7F0-63EC-49FB-B3B0-6B1AF2547E7C.jpg");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "ImageName",
                value: null);
        }
    }
}
