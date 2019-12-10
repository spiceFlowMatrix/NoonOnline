using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class lessonassignmentsubmission_0808610pm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LessonAssignmentSubmissionFiles",
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
                    SubmissionId = table.Column<long>(nullable: false),
                    FileId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonAssignmentSubmissionFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LessonAssignmentSubmissions",
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
                    AssignmentId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    TeacherId = table.Column<long>(nullable: false),
                    Score = table.Column<long>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    IsSubmission = table.Column<bool>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonAssignmentSubmissions", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "DefaultValues",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 19L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 20L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 21L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 22L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 23L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 24L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 25L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 26L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 27L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 28L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 29L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 30L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 31L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 32L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 33L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 34L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 35L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 36L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 37L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 38L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 39L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 40L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 41L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 42L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 43L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 44L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 45L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 46L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 47L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 48L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 49L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "SalesTax",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "TimeIntervals",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 6:10:26 PM");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonAssignmentSubmissionFiles");

            migrationBuilder.DropTable(
                name: "LessonAssignmentSubmissions");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "DefaultValues",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 19L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 20L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 21L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 22L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 23L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 24L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 25L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 26L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 27L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 28L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 29L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 30L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 31L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 32L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 33L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 34L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 35L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 36L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 37L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 38L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 39L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 40L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 41L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 42L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 43L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 44L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 45L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 46L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 47L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 48L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 49L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "SalesTax",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "TimeIntervals",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "8/8/2019 5:40:00 PM");
        }
    }
}
