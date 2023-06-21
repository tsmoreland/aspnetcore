using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TennisByTheSea.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigurationEntity",
                columns: table => new
                {
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationEntity", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Courts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Forename = table.Column<string>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    _forename = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    _joinDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    _surname = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    _userId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourtMaintenanceSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkTitle = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CourtIsClosed = table.Column<bool>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CourtId = table.Column<int>(type: "INTEGER", nullable: false),
                    _courtId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourtMaintenanceSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourtMaintenanceSchedule_Courts_CourtId",
                        column: x => x.CourtId,
                        principalTable: "Courts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourtBookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    _memberId = table.Column<int>(type: "INTEGER", nullable: false),
                    _courtId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourtBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourtBookings_Courts__courtId",
                        column: x => x._courtId,
                        principalTable: "Courts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourtBookings_Members__memberId",
                        column: x => x._memberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourtBookings__courtId",
                table: "CourtBookings",
                column: "_courtId");

            migrationBuilder.CreateIndex(
                name: "IX_CourtBookings__memberId",
                table: "CourtBookings",
                column: "_memberId");

            migrationBuilder.CreateIndex(
                name: "IX_CourtMaintenanceSchedule_CourtId",
                table: "CourtMaintenanceSchedule",
                column: "CourtId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigurationEntity");

            migrationBuilder.DropTable(
                name: "CourtBookings");

            migrationBuilder.DropTable(
                name: "CourtMaintenanceSchedule");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Courts");
        }
    }
}
