using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarvedRock.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ItemOrderView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string sql = """
                CREATE VIEW customers_orders_view AS
                SELECT
                    orders.id as order_id
                    orders.name as order_name
                    customers.id as customer_id
                    CONCAT(customers.first_name, ' ', customers.last_name) as customer_name
                    items.description as item_description
                    items.price
                    items.price_after_vat
                FROM item_order
                JOIN items on item_order.items_id = items.id
                JOIN order on item_order.order_id = orders.id
                JOIN customers on customer.id = customers.id
                """;
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string sql = "DROP VIEW customer_orders_view";
            migrationBuilder.Sql(sql);
        }
    }
}
