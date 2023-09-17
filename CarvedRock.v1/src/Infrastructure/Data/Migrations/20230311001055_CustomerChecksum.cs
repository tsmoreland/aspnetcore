using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarvedRock.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CustomerChecksum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "checksum",
                table: "customers",
                type: "varbinary(max)",
                nullable: true,
                computedColumnSql: "CONVERT(VARBINARY(1024),CHECKSUM([username],[first_name],[last_name]))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "checksum",
                table: "customers");
        }
    }
}
