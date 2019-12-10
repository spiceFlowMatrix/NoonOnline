using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class added_task_management_tbls_17102019 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskActivityCategoryFeedBacks",
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
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskActivityCategoryFeedBacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskActivityFeedBacks",
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
                    TaskId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskActivityFeedBacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskCategoryFeedBacks",
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
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCategoryFeedBacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskFeedBacks",
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
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CategoryId = table.Column<long>(nullable: false),
                    GradeId = table.Column<long>(nullable: true),
                    CourseId = table.Column<long>(nullable: true),
                    ChapterId = table.Column<long>(nullable: true),
                    LessonId = table.Column<long>(nullable: true),
                    StartDate = table.Column<string>(nullable: true),
                    ComplatedDate = table.Column<string>(nullable: true),
                    Time = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Device = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    AppVersion = table.Column<string>(nullable: true),
                    OperatingSystem = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskFeedBacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskFileFeedBacks",
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
                    TaskId = table.Column<long>(nullable: false),
                    FileId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskFileFeedBacks", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "DefaultValues",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 19L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 20L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 21L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 22L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 23L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 24L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 25L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 26L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 27L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 28L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 29L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 30L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 31L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 32L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 33L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 34L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 35L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 36L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 37L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 38L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 39L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 40L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 41L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 42L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 43L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 44L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 45L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 46L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 47L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 48L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 49L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "SalesTax",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.InsertData(
                table: "TaskActivityCategoryFeedBacks",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId", "Name" },
                values: new object[,]
                {
                    { 1L, "10/17/2019 6:07:42 AM", 1, null, null, false, null, null, "In Queue" },
                    { 2L, "10/17/2019 6:07:42 AM", 1, null, null, false, null, null, "In Progress" },
                    { 3L, "10/17/2019 6:07:42 AM", 1, null, null, false, null, null, "Completed" },
                    { 4L, "10/17/2019 6:07:42 AM", 1, null, null, false, null, null, "Archived" }
                });

            migrationBuilder.InsertData(
                table: "TaskCategoryFeedBacks",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId", "Name" },
                values: new object[,]
                {
                    { 1L, "10/17/2019 6:07:42 AM", 1, null, null, false, null, null, "Something Isn't Working in the App" },
                    { 2L, "10/17/2019 6:07:42 AM", 1, null, null, false, null, null, "Video Issue" },
                    { 3L, "10/17/2019 6:07:42 AM", 1, null, null, false, null, null, "Text Issue" },
                    { 4L, "10/17/2019 6:07:42 AM", 1, null, null, false, null, null, "Lesson Assignment Issue" },
                    { 5L, "10/17/2019 6:07:42 AM", 1, null, null, false, null, null, "Chapter Assignment Issue" },
                    { 6L, "10/17/2019 6:07:42 AM", 1, null, null, false, null, null, "Quiz Issue" },
                    { 7L, "10/17/2019 6:07:42 AM", 1, null, null, false, null, null, "General Feedback" }
                });

            migrationBuilder.UpdateData(
                table: "TimeIntervals",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/17/2019 11:37:42 AM");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskActivityCategoryFeedBacks");

            migrationBuilder.DropTable(
                name: "TaskActivityFeedBacks");

            migrationBuilder.DropTable(
                name: "TaskCategoryFeedBacks");

            migrationBuilder.DropTable(
                name: "TaskFeedBacks");

            migrationBuilder.DropTable(
                name: "TaskFileFeedBacks");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "AddtionalServices",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "DefaultValues",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "ERPAccounts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 19L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 20L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 21L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 22L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 23L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 24L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 25L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 26L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 27L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 28L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 29L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 30L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 31L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 32L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 33L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 34L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 35L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 36L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 37L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 38L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 39L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 40L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 41L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 42L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 43L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 44L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 45L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 46L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 47L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 48L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 49L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "SalesTax",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "TimeIntervals",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "10/2/2019 4:53:14 PM");
        }
    }
}
