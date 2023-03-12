using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarvedRock.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_orders_name",
                table: "orders",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_customers_first_name_last_name",
                table: "customers",
                columns: new[] { "first_name", "last_name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_orders_name",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_customers_first_name_last_name",
                table: "customers");
        }
    }
}
