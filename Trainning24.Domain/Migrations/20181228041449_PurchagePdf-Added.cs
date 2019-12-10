using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class PurchagePdfAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchagePdf",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    PurchageId = table.Column<long>(nullable: false),
                    PdfFile = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchagePdf", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "DefaultValues",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/28/2018 9:44:48 AM");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchagePdf");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "DefaultValues",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/26/2018 10:45:38 AM");
        }
    }
}
