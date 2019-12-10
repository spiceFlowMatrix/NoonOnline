using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class AddtionalServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddtionalServices",
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
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddtionalServices", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "DefaultValues",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 19L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 20L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 21L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 22L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 23L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 24L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 25L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 26L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 27L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 28L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 29L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 30L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 31L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 32L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 33L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 34L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 35L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 36L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/22/2019 10:21:56 AM");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddtionalServices");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "DefaultValues",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 19L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 20L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 21L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 22L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 23L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 24L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 25L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 26L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 27L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 28L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 29L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 30L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 31L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 32L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 33L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 34L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 35L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 36L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 3:36:46 PM");
        }
    }
}
