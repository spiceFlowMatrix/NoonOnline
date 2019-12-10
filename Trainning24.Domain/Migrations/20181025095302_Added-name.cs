using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class Addedname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "DocumentDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "DocumentDetails");
        }
    }
}
