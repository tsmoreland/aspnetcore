using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarvedRock.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class IndirectManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // nothing to do when moving to indirect references, this is here only to show that
            // this is the goal at least, when moving from skip navigation with payload to indirect many-to-many
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
