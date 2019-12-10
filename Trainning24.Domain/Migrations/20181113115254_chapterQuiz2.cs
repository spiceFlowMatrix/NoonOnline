using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class chapterQuiz2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreationTime",
                table: "chapterQuiz",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatorUserId",
                table: "chapterQuiz",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeleterUserId",
                table: "chapterQuiz",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletionTime",
                table: "chapterQuiz",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "chapterQuiz",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModificationTime",
                table: "chapterQuiz",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifierUserId",
                table: "chapterQuiz",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "chapterQuiz");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "chapterQuiz");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "chapterQuiz");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "chapterQuiz");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "chapterQuiz");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "chapterQuiz");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "chapterQuiz");
        }
    }
}
