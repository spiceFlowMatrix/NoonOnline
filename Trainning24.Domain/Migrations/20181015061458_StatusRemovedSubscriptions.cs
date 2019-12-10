using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class StatusRemovedSubscriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Subscriptions");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "SubscriptionMetadata",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "SubscriptionMetadata");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Subscriptions",
                nullable: true);
        }
    }
}
