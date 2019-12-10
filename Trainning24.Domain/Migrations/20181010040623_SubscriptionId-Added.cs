using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class SubscriptionIdAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubscriptionId",
                table: "SubscriptionMetadata",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "SubscriptionMetadata");
        }
    }
}
