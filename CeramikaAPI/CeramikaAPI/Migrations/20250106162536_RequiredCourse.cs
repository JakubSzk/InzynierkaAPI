using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CeramikaAPI.Migrations
{
    /// <inheritdoc />
    public partial class RequiredCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Courses_Name",
                table: "Courses");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_When",
                table: "Courses",
                column: "When",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Courses_When",
                table: "Courses");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Name",
                table: "Courses",
                column: "Name",
                unique: true);
        }
    }
}
