using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarvedRock.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class NET9_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "item_type",
                table: "items",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "UncatagorizedItem",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldDefaultValue: "UncatagorizedItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "item_type",
                table: "items",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "UncatagorizedItem",
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21,
                oldDefaultValue: "UncatagorizedItem");
        }
    }
}
