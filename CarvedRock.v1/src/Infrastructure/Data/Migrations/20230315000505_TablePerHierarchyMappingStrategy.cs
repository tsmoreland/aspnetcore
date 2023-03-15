using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarvedRock.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class TablePerHierarchyMappingStrategy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "canning_material",
                table: "items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "expiry_date",
                table: "items",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fabric",
                table: "items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "item_type",
                table: "items",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "UncatagorizedItem");

            migrationBuilder.AddColumn<DateTime>(
                name: "production_date",
                table: "items",
                type: "datetime2",
                nullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AddColumn<string>(
                name: "publication_frequency",
                table: "items",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "canning_material",
                table: "items");

            migrationBuilder.DropColumn(
                name: "expiry_date",
                table: "items");

            migrationBuilder.DropColumn(
                name: "fabric",
                table: "items");

            migrationBuilder.DropColumn(
                name: "item_type",
                table: "items");

            migrationBuilder.DropColumn(
                name: "production_date",
                table: "items");

            migrationBuilder.DropColumn(
                name: "publication_frequency",
                table: "items");
        }
    }
}
