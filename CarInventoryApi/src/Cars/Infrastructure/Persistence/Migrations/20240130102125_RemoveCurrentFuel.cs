using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarInventory.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCurrentFuel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "current_fuel",
                table: "cars");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "current_fuel",
                table: "cars",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
