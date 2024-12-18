using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workify.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyNameToJobListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Skills",
                table: "JobListings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Skills",
                table: "JobListings");
        }
    }
}
