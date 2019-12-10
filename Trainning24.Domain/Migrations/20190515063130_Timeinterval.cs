using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class Timeinterval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeIntervals",
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
                    Interval = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeIntervals", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "DefaultValues",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 19L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 20L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 21L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 22L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 23L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 24L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 25L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 26L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 27L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 28L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 29L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 30L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 31L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 32L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 33L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 34L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 35L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 36L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 37L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 38L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 39L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 40L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 41L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 42L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 43L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 44L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 45L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 46L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 47L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 48L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 49L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.InsertData(
                table: "TimeIntervals",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "Interval", "IsDeleted", "LastModificationTime", "LastModifierUserId" },
                values: new object[] { 1L, "5/15/2019 12:01:29 PM", 1, null, null, 5, false, null, null });

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/15/2019 12:01:29 PM");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeIntervals");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "DefaultValues",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 19L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 20L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 21L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 22L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 23L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 24L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 25L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 26L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 27L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 28L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 29L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 30L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 31L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 32L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 33L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 34L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 35L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 36L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 37L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 38L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 39L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 40L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 41L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 42L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 43L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 44L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 45L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 46L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 47L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 48L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 49L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "5/4/2019 11:47:23 AM");
        }
    }
}
