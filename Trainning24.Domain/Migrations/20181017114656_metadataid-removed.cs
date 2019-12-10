using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class metadataidremoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "metadataid",
                table: "SchoolDetails");

            migrationBuilder.DropColumn(
                name: "metadataid",
                table: "IndividualDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "metadataid",
                table: "SchoolDetails",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "metadataid",
                table: "IndividualDetails",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
