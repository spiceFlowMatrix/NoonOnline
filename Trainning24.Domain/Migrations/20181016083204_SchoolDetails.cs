using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class SchoolDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SchoolDetails",
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
                    RegisterNumber = table.Column<string>(nullable: true),
                    SchoolTypeId = table.Column<int>(nullable: false),
                    SchoolName = table.Column<string>(nullable: true),
                    SchoolAddress = table.Column<string>(nullable: true),
                    SectionTypeId = table.Column<int>(nullable: false),
                    NumberOfTeacherMale = table.Column<int>(nullable: false),
                    NumberOfTeacherFemale = table.Column<int>(nullable: false),
                    NumberOfStudentMale = table.Column<int>(nullable: false),
                    NumberOfStudentFemale = table.Column<int>(nullable: false),
                    NumberOfStaffMale = table.Column<int>(nullable: false),
                    NumberOfStaffFemale = table.Column<int>(nullable: false),
                    PowerAddressId = table.Column<int>(nullable: false),
                    InternetAccessId = table.Column<int>(nullable: false),
                    BuildingOwnershipId = table.Column<int>(nullable: false),
                    Computers = table.Column<string>(nullable: true),
                    Monitors = table.Column<string>(nullable: true),
                    Routers = table.Column<string>(nullable: true),
                    Dongles3G = table.Column<string>(nullable: true),
                    SchoolLicense = table.Column<long>(nullable: false),
                    RegisterationPaper = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolDetails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchoolDetails");
        }
    }
}
