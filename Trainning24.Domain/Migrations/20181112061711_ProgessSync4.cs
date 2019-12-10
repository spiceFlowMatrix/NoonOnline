using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class ProgessSync4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreationTime",
                table: "progessSync",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatorUserId",
                table: "progessSync",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeleterUserId",
                table: "progessSync",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletionTime",
                table: "progessSync",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "progessSync",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModificationTime",
                table: "progessSync",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifierUserId",
                table: "progessSync",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "progessSync");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "progessSync");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "progessSync");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "progessSync");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "progessSync");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "progessSync");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "progessSync");
        }
    }
}
