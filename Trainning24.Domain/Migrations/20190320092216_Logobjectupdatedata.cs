using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class Logobjectupdatedata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "DefaultValues",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.InsertData(
                table: "LogObjectTypes",
                columns: new[] { "Id", "Action", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "EntityType", "IsDeleted", "LastModificationTime", "LastModifierUserId" },
                values: new object[,]
                {
                    { 24L, "Delete", "3/20/2019 2:52:15 PM", 1, null, null, "Notification", false, null, null },
                    { 23L, "Update", "3/20/2019 2:52:15 PM", 1, null, null, "Notification", false, null, null },
                    { 22L, "Create", "3/20/2019 2:52:15 PM", 1, null, null, "Notification", false, null, null },
                    { 21L, "Delete", "3/20/2019 2:52:15 PM", 1, null, null, "File", false, null, null },
                    { 20L, "Update", "3/20/2019 2:52:15 PM", 1, null, null, "File", false, null, null },
                    { 19L, "Create", "3/20/2019 2:52:15 PM", 1, null, null, "File", false, null, null },
                    { 18L, "Delete", "3/20/2019 2:52:15 PM", 1, null, null, "Question", false, null, null },
                    { 17L, "Update", "3/20/2019 2:52:15 PM", 1, null, null, "Question", false, null, null },
                    { 16L, "Create", "3/20/2019 2:52:15 PM", 1, null, null, "Question", false, null, null },
                    { 15L, "Delete", "3/20/2019 2:52:15 PM", 1, null, null, "Quiz", false, null, null },
                    { 13L, "Create", "3/20/2019 2:52:15 PM", 1, null, null, "Quiz", false, null, null },
                    { 12L, "Delete", "3/20/2019 2:52:15 PM", 1, null, null, "Lesson", false, null, null },
                    { 11L, "Update", "3/20/2019 2:52:15 PM", 1, null, null, "Lesson", false, null, null },
                    { 10L, "Create", "3/20/2019 2:52:15 PM", 1, null, null, "Lesson", false, null, null },
                    { 9L, "Delete", "3/20/2019 2:52:15 PM", 1, null, null, "Chapter", false, null, null },
                    { 8L, "Update", "3/20/2019 2:52:15 PM", 1, null, null, "Chapter", false, null, null },
                    { 7L, "Create", "3/20/2019 2:52:15 PM", 1, null, null, "Chapter", false, null, null },
                    { 6L, "Delete", "3/20/2019 2:52:15 PM", 1, null, null, "User", false, null, null },
                    { 5L, "Update", "3/20/2019 2:52:15 PM", 1, null, null, "User", false, null, null },
                    { 4L, "Create", "3/20/2019 2:52:15 PM", 1, null, null, "User", false, null, null },
                    { 14L, "Update", "3/20/2019 2:52:15 PM", 1, null, null, "Quiz", false, null, null }
                });

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 2:52:15 PM");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 23L);

            migrationBuilder.DeleteData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 24L);

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Buckets",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "DefaultValues",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "FeedBackTaskStatusOption",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "LogObjectTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "3/20/2019 10:56:20 AM");
        }
    }
}
