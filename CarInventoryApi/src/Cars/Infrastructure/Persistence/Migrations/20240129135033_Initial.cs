using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarInventory.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cars",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    make = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    model = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    horse_power = table.Column<int>(type: "INTEGER", nullable: false),
                    engine_type = table.Column<int>(type: "INTEGER", nullable: false),
                    fuel_capacity = table.Column<double>(type: "REAL", nullable: false),
                    current_fuel = table.Column<double>(type: "REAL", nullable: false, defaultValue: 0.0),
                    number_of_doors = table.Column<int>(type: "INTEGER", nullable: false),
                    mpg = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cars", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cars_make_model",
                table: "cars",
                columns: new[] { "make", "model" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cars");
        }
    }
}
