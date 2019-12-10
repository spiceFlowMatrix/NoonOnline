using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class BucketTableWithSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buckets",
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
                    BucketName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buckets", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Buckets",
                columns: new[] { "Id", "BucketName", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId" },
                values: new object[,]
                {
                    { 1L, "core-api-sql-migrations", "12/7/2018 6:16:02 PM", 1, null, null, false, null, null },
                    { 11L, "t24-secure-files", "12/7/2018 6:16:02 PM", 1, null, null, false, null, null },
                    { 9L, "t24-primary-pdf-storage", "12/7/2018 6:16:02 PM", 1, null, null, false, null, null },
                    { 8L, "t24-primary-image-storage", "12/7/2018 6:16:02 PM", 1, null, null, false, null, null },
                    { 7L, "t24-primary-audio-storage", "12/7/2018 6:16:02 PM", 1, null, null, false, null, null },
                    { 10L, "t24-primary-video-storage", "12/7/2018 6:16:02 PM", 1, null, null, false, null, null },
                    { 5L, "eu.artifacts.training24-197210.appspot.com", "12/7/2018 6:16:02 PM", 1, null, null, false, null, null },
                    { 4L, "edg-sales-primary-storage", "12/7/2018 6:16:02 PM", 1, null, null, false, null, null },
                    { 3L, "edg-primary-profile-image-storage", "12/7/2018 6:16:02 PM", 1, null, null, false, null, null },
                    { 2L, "edg-primary-course-image-storage", "12/7/2018 6:16:02 PM", 1, null, null, false, null, null },
                    { 6L, "t24-app-builds", "12/7/2018 6:16:02 PM", 1, null, null, false, null, null }
                });

            migrationBuilder.UpdateData(
                table: "DefaultValues",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/7/2018 6:16:02 PM");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Buckets");

            migrationBuilder.UpdateData(
                table: "DefaultValues",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "FileTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: "12/7/2018 5:09:25 PM");
        }
    }
}
