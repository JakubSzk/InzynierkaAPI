using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CeramikaAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangedCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "Courses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Length",
                table: "Courses");
        }
    }
}
