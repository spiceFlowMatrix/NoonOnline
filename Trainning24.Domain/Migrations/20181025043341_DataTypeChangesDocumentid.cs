using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class DataTypeChangesDocumentid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "Deposit");

            migrationBuilder.AddColumn<string>(
                name: "DocumentIds",
                table: "Deposit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentIds",
                table: "Deposit");

            migrationBuilder.AddColumn<long>(
                name: "DocumentId",
                table: "Deposit",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
