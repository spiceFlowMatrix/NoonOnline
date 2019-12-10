using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class removeteacherid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "AssignmentStudent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TeacherId",
                table: "AssignmentStudent",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
