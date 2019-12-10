using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class SchoolTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SchoolId",
                table: "Grade",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "School",
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
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_School", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "School");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Grade");
        }
    }
}
