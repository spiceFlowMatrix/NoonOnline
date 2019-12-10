using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class Quizsummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuizSummary",
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
                    StudentId = table.Column<long>(nullable: false),
                    QuizId = table.Column<long>(nullable: false),
                    QSummary = table.Column<int>(nullable: false),
                    Attempts = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizSummary", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizSummary");
        }
    }
}
