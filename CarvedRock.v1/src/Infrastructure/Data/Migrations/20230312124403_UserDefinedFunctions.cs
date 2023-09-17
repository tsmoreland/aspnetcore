using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarvedRock.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class UserDefinedFunctions : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            CREATE FUNCTION get_customer_orders_by_id(@customer_id int)
            RETURNS TABLE
            AS
            RETURN
            (
                SELECT
                    orders.id as orders_id
                    orders.name as orders_name
                    customers.id as customers_id
                    CONCAT(customers.first_name, ' ', customers.last_name) as customer_name
                    items.id as item_id
                    items.description as item_description
                    item.price,
                    item.price_after_vat
                FROM item_order
                JOIN items ON item_order.item_id = items.id
                JOIN orders ON item_order.orders_id = orders.id
                JOIN customers ON item_order.customer_id = customers.id
                WHERE customers.id = @customer_id
            """);
        migrationBuilder.Sql("""
            CREATE FUNCTION get_status_overview()
            RETURNS
            @statuses TABLE
            (
                id int
                status NVARCHAR(max)
                source NVARCHAR(max)
            )
            AS
            BEGIN
                INSERT INTO @statuses
                SELECT id, status, 'Customers' as source
                FROM customers

                INSERT INTO @statuses
                SELECT id, status, 'Orders' as source
                FROM orders
                RETURN
            END
            """);
        migrationBuilder.Sql("""
            CREATE FUNCTION get_total_invoice_amount_by_order_id(@order_id int)
            RETURNS int
            AS
            BEGIN
            (
                SELECT SUM(items.price)
                FROM orders
                JOIN item_order on orders.id = item_order.orders_id
                JOIN items on item_order.items_id = items.id
                WHERE orders.id = @order_id
            )
            END
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP FUNCTION get_customer_orders_by_id");
        migrationBuilder.Sql("DROP FUNCTION get_status_overview");
        migrationBuilder.Sql("DROP FUNCTION get_total_invoice_amount_by_order_id");
    }
}
