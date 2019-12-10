using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class TableNameChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_progessSync",
                table: "progessSync");

            migrationBuilder.RenameTable(
                name: "progessSync",
                newName: "ProgessSync");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProgessSync",
                table: "ProgessSync",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProgessSync",
                table: "ProgessSync");

            migrationBuilder.RenameTable(
                name: "ProgessSync",
                newName: "progessSync");

            migrationBuilder.AddPrimaryKey(
                name: "PK_progessSync",
                table: "progessSync",
                column: "Id");
        }
    }
}
