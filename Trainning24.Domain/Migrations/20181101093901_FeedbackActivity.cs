using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class FeedbackActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FeedBackTask",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileLink",
                table: "FeedBackTask",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "FeedBackTask");

            migrationBuilder.DropColumn(
                name: "FileLink",
                table: "FeedBackTask");
        }
    }
}
