using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class ChapterIdAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Feedback");

            migrationBuilder.AddColumn<long>(
                name: "ChapterId",
                table: "Feedback",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChapterId",
                table: "Feedback");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Feedback",
                nullable: true);
        }
    }
}
