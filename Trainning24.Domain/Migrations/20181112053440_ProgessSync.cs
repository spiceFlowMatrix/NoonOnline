using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class ProgessSync : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "progessSync",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GradeId = table.Column<long>(nullable: false),
                    IsStatus = table.Column<bool>(nullable: false),
                    LessionProgessId = table.Column<long>(nullable: false),
                    LessionId = table.Column<long>(nullable: false),
                    LessionProgess = table.Column<decimal>(nullable: false),
                    QuizId = table.Column<long>(nullable: false),
                    TotalRecords = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_progessSync", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "progessSync");
        }
    }
}
