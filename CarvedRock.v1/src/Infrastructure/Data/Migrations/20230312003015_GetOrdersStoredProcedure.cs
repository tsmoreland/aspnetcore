using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarvedRock.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class GetOrdersStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE PROCEDURE GetOrders
                AS
                BEGIN
                    SELECT * FROM orders
                END
                GO
                """);

            migrationBuilder.Sql("""
                CREATE PROCEDURE GetOrdersById
                    @customer_id int,
                    @customer_order_count int OUTPUT
                AS
                BEGIN
                    SELECT * FROM orders WHERE customer_id = @customer_id
                    SET @customer_order_count = (SELECT COUNT(*) from orders WHERE customer_id = @customer_id
                END 
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP PROCDURE GetOrders;
                """);
            migrationBuilder.Sql("""
                DROP PROCDURE GetOrdersById;
                """);

        }
    }
}
