using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class ProgressSync4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LessionProgessId",
                table: "progessSync",
                newName: "LessonProgressId");

            migrationBuilder.RenameColumn(
                name: "LessionProgess",
                table: "progessSync",
                newName: "LessonProgress");

            migrationBuilder.RenameColumn(
                name: "LessionId",
                table: "progessSync",
                newName: "LessonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LessonProgressId",
                table: "progessSync",
                newName: "LessionProgessId");

            migrationBuilder.RenameColumn(
                name: "LessonProgress",
                table: "progessSync",
                newName: "LessionProgess");

            migrationBuilder.RenameColumn(
                name: "LessonId",
                table: "progessSync",
                newName: "LessionId");
        }
    }
}
