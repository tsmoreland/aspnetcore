using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroShop.Services.Coupons.CouponApiApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class NormalizedCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Coupons",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedCode",
                table: "Coupons",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 1,
                column: "NormalizedCode",
                value: "10OFF");

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 2,
                column: "NormalizedCode",
                value: "20OFF");

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 3,
                column: "NormalizedCode",
                value: "25OFF");

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 4,
                column: "NormalizedCode",
                value: "50OFF");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_NormalizedCode",
                table: "Coupons",
                column: "NormalizedCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Coupons_NormalizedCode",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "NormalizedCode",
                table: "Coupons");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);
        }
    }
}
