using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class newFieldsAddedSalesAgent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgreedMonthlyDeposit",
                table: "SalesAgent",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "CurrencyId",
                table: "SalesAgent",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "FocalPoint",
                table: "SalesAgent",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartnerBackgroud",
                table: "SalesAgent",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhysicalAddress",
                table: "SalesAgent",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "SalesAgent",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgreedMonthlyDeposit",
                table: "SalesAgent");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "SalesAgent");

            migrationBuilder.DropColumn(
                name: "FocalPoint",
                table: "SalesAgent");

            migrationBuilder.DropColumn(
                name: "PartnerBackgroud",
                table: "SalesAgent");

            migrationBuilder.DropColumn(
                name: "PhysicalAddress",
                table: "SalesAgent");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "SalesAgent");
        }
    }
}
