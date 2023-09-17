using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BethanysPieShop.Admin.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bas");

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "bas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    concurrency_token = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "bas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    address_line_1 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    address_line_2 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    post_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    city = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    order_total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    order_placed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    concurrency_token = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pies",
                schema: "bas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    short_description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    long_description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    allergy_information = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    image_filename = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    image_thumbnail_filename = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    is_pie_of_the_week = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    in_stock = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    concurrency_token = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pies_Categories_category_id",
                        column: x => x.category_id,
                        principalSchema: "bas",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Ingredient",
                schema: "bas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    amount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    concurrency_token = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    PieId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredient_Pies_PieId",
                        column: x => x.PieId,
                        principalSchema: "bas",
                        principalTable: "Pies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                schema: "bas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    pie_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    amount = table.Column<int>(type: "int", precision: 18, scale: 2, nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    concurrency_token = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order_order_id",
                        column: x => x.order_id,
                        principalSchema: "bas",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Pies_pie_id",
                        column: x => x.pie_id,
                        principalSchema: "bas",
                        principalTable: "Pies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_PieId",
                schema: "bas",
                table: "Ingredient",
                column: "PieId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_email",
                schema: "bas",
                table: "Orders",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_Order_last_name",
                schema: "bas",
                table: "Orders",
                column: "last_name");

            migrationBuilder.CreateIndex(
                name: "IX_Order_post_code",
                schema: "bas",
                table: "Orders",
                column: "post_code");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_order_id",
                schema: "bas",
                table: "OrderDetail",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_pie_id",
                schema: "bas",
                table: "OrderDetail",
                column: "pie_id");

            migrationBuilder.CreateIndex(
                name: "IX_Pies_category_id",
                schema: "bas",
                table: "Pies",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Pies_name",
                schema: "bas",
                table: "Pies",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_Pies_price",
                schema: "bas",
                table: "Pies",
                column: "price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingredient",
                schema: "bas");

            migrationBuilder.DropTable(
                name: "OrderDetail",
                schema: "bas");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "bas");

            migrationBuilder.DropTable(
                name: "Pies",
                schema: "bas");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "bas");
        }
    }
}
