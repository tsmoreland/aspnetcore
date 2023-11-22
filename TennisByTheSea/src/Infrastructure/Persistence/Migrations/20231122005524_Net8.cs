using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TennisByTheSea.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Net8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_forename",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "_surname",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "_joinDate",
                table: "Members",
                newName: "JoinDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JoinDate",
                table: "Members",
                newName: "_joinDate");

            migrationBuilder.AddColumn<string>(
                name: "_forename",
                table: "Members",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "_surname",
                table: "Members",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
