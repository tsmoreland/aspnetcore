using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GloboTicket.Shop.Catalog.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Concerts",
                columns: new[] { "ConcertId", "Artist", "ConcurrencyToken", "Date", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("3448d5a4-0f72-4dd7-bf15-c14a46b26c00"), "Michael Johnson", "475ed4ab8df24bf493916ce9eb1e82ed", new DateTime(2023, 11, 12, 18, 47, 35, 205, DateTimeKind.Local).AddTicks(5912), "Michael Johnson doesn't need an introduction. His 25 concert across the globe last year were seen by thousands. Can we add you to the list?", "/img/michael.jpg", "The State of Affairs: Michael Live!", 0 },
                    { new Guid("62787623-4c52-43fe-b0c9-b7044fb5929b"), "Manuel Santinonisi", "9deb7ce8579e4e57a7b3240111a3ad02", new DateTime(2023, 6, 12, 18, 47, 35, 205, DateTimeKind.Local).AddTicks(5937), "Get on the hype of Spanish Guitar concerts with Manuel.", "/img/guitar.jpg", "Spanish guitar hits with Manuel", 0 },
                    { new Guid("adc42c09-08c1-4d2c-9f96-2d15bb1af299"), "Nick Sailor", "6992b812e2444f25ad3945e953a2f731", new DateTime(2023, 10, 12, 18, 47, 35, 205, DateTimeKind.Local).AddTicks(5945), "The critics are over the moon and so will you after you've watched this sing and dance extravaganza written by Nick Sailor, the man from 'My dad and sister'.", "/img/musical.jpg", "To the Moon and Back", 0 },
                    { new Guid("b419a7ca-3321-4f38-be8e-4d7b6a529319"), "DJ 'The Mike'", "033a61e9e35b4121b5296acc4eab5d5e", new DateTime(2023, 6, 12, 18, 47, 35, 205, DateTimeKind.Local).AddTicks(5922), "DJs from all over the world will compete in this epic battle for eternal fame.", "/img/dj.jpg", "Clash of the DJs", 0 },
                    { new Guid("ee272f8b-6096-4cb6-8625-bb4bb2d89e8b"), "John Egbert", "bb94d43c5d2540d3ab933df90565cc86", new DateTime(2023, 8, 12, 18, 47, 35, 205, DateTimeKind.Local).AddTicks(5821), "Join John for his farwell tour across 15 continents. John really needs no introduction since he has already mesmerized the world with his banjo.", "/img/banjo.jpg", "John Egbert Live", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Concerts",
                keyColumn: "ConcertId",
                keyValue: new Guid("3448d5a4-0f72-4dd7-bf15-c14a46b26c00"));

            migrationBuilder.DeleteData(
                table: "Concerts",
                keyColumn: "ConcertId",
                keyValue: new Guid("62787623-4c52-43fe-b0c9-b7044fb5929b"));

            migrationBuilder.DeleteData(
                table: "Concerts",
                keyColumn: "ConcertId",
                keyValue: new Guid("adc42c09-08c1-4d2c-9f96-2d15bb1af299"));

            migrationBuilder.DeleteData(
                table: "Concerts",
                keyColumn: "ConcertId",
                keyValue: new Guid("b419a7ca-3321-4f38-be8e-4d7b6a529319"));

            migrationBuilder.DeleteData(
                table: "Concerts",
                keyColumn: "ConcertId",
                keyValue: new Guid("ee272f8b-6096-4cb6-8625-bb4bb2d89e8b"));
        }
    }
}
