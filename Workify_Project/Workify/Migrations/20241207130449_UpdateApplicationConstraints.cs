using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workify.Migrations
{
    /// <inheritdoc />
    public partial class UpdateApplicationConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_JobSeekers_JobSeekerId",
                table: "Applications");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_JobSeekers_JobSeekerId",
                table: "Applications",
                column: "JobSeekerId",
                principalTable: "JobSeekers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_JobSeekers_JobSeekerId",
                table: "Applications");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_JobSeekers_JobSeekerId",
                table: "Applications",
                column: "JobSeekerId",
                principalTable: "JobSeekers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
