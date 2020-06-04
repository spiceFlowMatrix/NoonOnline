using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class _03062020_AppVersionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppVersion",
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
                    Version = table.Column<string>(nullable: true),
                    IsForceUpdate = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppVersion", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.InsertData(
                table: "AppVersion",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "IsForceUpdate", "LastModificationTime", "LastModifierUserId", "Version" },
                values: new object[] { 1L, "06/03/20 5:20:15 PM", 1, null, null, false, false, null, null, "1.0" });

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "DefaultValues",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 19L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 20L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 21L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 22L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 23L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 24L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 25L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 26L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 27L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 28L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 29L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 30L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 31L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 32L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 33L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 34L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 35L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 36L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 37L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 38L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 39L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 40L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 41L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 42L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 43L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 44L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 45L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 46L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 47L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 48L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 49L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "SalesTax",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "TaskActivityCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "06/03/20 11:50:15 AM");

            migrationBuilder.UpdateData(
                table: "TaskActivityCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "06/03/20 11:50:15 AM");

            migrationBuilder.UpdateData(
                table: "TaskActivityCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "06/03/20 11:50:15 AM");

            migrationBuilder.UpdateData(
                table: "TaskActivityCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "06/03/20 11:50:15 AM");

            migrationBuilder.UpdateData(
                table: "TaskCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "06/03/20 11:50:15 AM");

            migrationBuilder.UpdateData(
                table: "TaskCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "06/03/20 11:50:15 AM");

            migrationBuilder.UpdateData(
                table: "TaskCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "06/03/20 11:50:15 AM");

            migrationBuilder.UpdateData(
                table: "TaskCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "06/03/20 11:50:15 AM");

            migrationBuilder.UpdateData(
                table: "TaskCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "06/03/20 11:50:15 AM");

            migrationBuilder.UpdateData(
                table: "TaskCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "06/03/20 11:50:15 AM");

            migrationBuilder.UpdateData(
                table: "TaskCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "06/03/20 11:50:15 AM");

            migrationBuilder.UpdateData(
                table: "TimeIntervals",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "06/03/20 5:20:15 PM");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppVersion");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "DefaultValues",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 19L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 20L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 21L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 22L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 23L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 24L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 25L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 26L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 27L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 28L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 29L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 30L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 31L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 32L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 33L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 34L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 35L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 36L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 37L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 38L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 39L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 40L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 41L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 42L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 43L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 44L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 45L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 46L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 47L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 48L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 49L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "SalesTax",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "TaskActivityCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "05/28/20 4:50:23 AM");

            migrationBuilder.UpdateData(
                table: "TaskActivityCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "05/28/20 4:50:23 AM");

            migrationBuilder.UpdateData(
                table: "TaskActivityCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "05/28/20 4:50:23 AM");

            migrationBuilder.UpdateData(
                table: "TaskActivityCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "05/28/20 4:50:23 AM");

            migrationBuilder.UpdateData(
                table: "TaskCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "05/28/20 4:50:23 AM");

            migrationBuilder.UpdateData(
                table: "TaskCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "05/28/20 4:50:23 AM");

            migrationBuilder.UpdateData(
                table: "TaskCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "05/28/20 4:50:23 AM");

            migrationBuilder.UpdateData(
                table: "TaskCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "05/28/20 4:50:23 AM");

            migrationBuilder.UpdateData(
                table: "TaskCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "05/28/20 4:50:23 AM");

            migrationBuilder.UpdateData(
                table: "TaskCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "05/28/20 4:50:23 AM");

            migrationBuilder.UpdateData(
                table: "TaskCategoryFeedBacks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "05/28/20 4:50:23 AM");

            migrationBuilder.UpdateData(
                table: "TimeIntervals",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "05/28/20 10:20:23 AM");
        }
    }
}
