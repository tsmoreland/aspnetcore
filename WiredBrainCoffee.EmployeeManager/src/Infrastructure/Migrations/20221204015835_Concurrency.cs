using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WiredBrainCoffee.EmployeeManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Concurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string triggerFormat = """
                CREATE TRIGGER Set{0}RowVersion{1}
                AFTER {1} ON {0}
                BEGIN
                    UPDATE {0}
                    SET Version = CAST(ROUND((julianday('now') - 2440587.5)*86400000) AS INT)
                    WHERE rowid = NEW.rowid;
                END
                """;

            migrationBuilder.AddColumn<long>(
                name: "LastModifiedTime",
                table: "Employees",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<ulong>(
                name: "Version",
                table: "Employees",
                type: "INTEGER",
                rowVersion: true,
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<long>(
                name: "LastModifiedTime",
                table: "Departments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<ulong>(
                name: "Version",
                table: "Departments",
                type: "INTEGER",
                rowVersion: true,
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.Sql(string.Format(triggerFormat, "Employees", "INSERT"));
            migrationBuilder.Sql(string.Format(triggerFormat, "Employees", "UPDATE"));

            migrationBuilder.Sql(string.Format(triggerFormat, "Departments", "INSERT"));
            migrationBuilder.Sql(string.Format(triggerFormat, "Departments", "UPDATE"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string triggerFormat = "DROP TRIGGER Set{0}RowVersion{1}";

            migrationBuilder.DropColumn(
                name: "LastModifiedTime",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LastModifiedTime",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Departments");

            migrationBuilder.Sql(string.Format(triggerFormat, "Employees", "INSERT"));
            migrationBuilder.Sql(string.Format(triggerFormat, "Employees", "UPDATE"));

            migrationBuilder.Sql(string.Format(triggerFormat, "Departments", "INSERT"));
            migrationBuilder.Sql(string.Format(triggerFormat, "Departments", "UPDATE"));
        }
    }
}
