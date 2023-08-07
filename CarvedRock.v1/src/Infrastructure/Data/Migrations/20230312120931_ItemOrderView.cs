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
                    orders.id as orders_id
                    orders.name as orders_name
                    customers.id as customers_id
                    CONCAT(customers.first_name, ' ', customers.last_name) as customers_name
                    items.id as item_id
                    items.description as item_description
                    items.price
                    items.price_after_vat
                FROM item_order
                JOIN items on item_order.items_id = items.id
                JOIN order on item_order.orders_id = orders.id
                JOIN customers on item_order.customers_id = customers.id
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
