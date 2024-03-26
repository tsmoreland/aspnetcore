using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MicroShop.Services.Products.ProductsApiApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NormalizedCategoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryName", "Description", "ImageUrl", "Name", "NormalizedCategoryName", "NormalizedName", "Price" },
                values: new object[,]
                {
                    { 1, "Electronics", "Travel light and stay rugged without sacrificing an inch of speed and power with the smart, slim, lightweight, and military-grade durable IdeaPad Slim 5", "https://m.media-amazon.com/images/W/AVIF_800250-T3/images/I/71j4CpWJMHL._AC_SX425_.jpg", "Lenovo IdeaPad Slim 5", "ELECTRONICS", "LENOVO IDEAPAD SLIM 5", 549.99000000000001 },
                    { 2, "Electronics", "SUPERCHARGED BY M3 PRO OR M3 MAX — The Apple M3 Pro chip, with an up to 12-core CPU and up to 18-core GPU", "https://m.media-amazon.com/images/W/AVIF_800250-T3/images/I/61RnQM+JDTL._AC_SX679_.jpg", "Apple 2023 MacBook Pro laptop M3 Pro", "ELECTRONICS", "APPLE 2023 MACBOOK PRO LAPTOP M3 PRO", 1899.97 },
                    { 3, "Electronics", "16-inch high-performance gaming laptop with the latest CPU and GPU offerings", "https://m.media-amazon.com/images/W/AVIF_800250-T3/images/I/81FV-rLoN8L._AC_SX425_.jpg", "Alienware M16 Gaming Laptop - 16-inch", "ELECTRONICS", "ALIENWARE M16 GAMING LAPTOP - 16-INCH", 2729.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_NormalizedCategoryName",
                table: "Products",
                column: "NormalizedCategoryName");

            migrationBuilder.CreateIndex(
                name: "IX_Products_NormalizedName",
                table: "Products",
                column: "NormalizedName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
