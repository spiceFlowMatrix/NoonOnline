using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class DocumentDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentDetails",
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
                    DocumentUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IndividualDetails",
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
                    StudentCode = table.Column<string>(nullable: true),
                    StudentName = table.Column<string>(nullable: true),
                    SchoolName = table.Column<string>(nullable: true),
                    FatherNumber = table.Column<string>(nullable: true),
                    IdCardNumber = table.Column<string>(nullable: true),
                    PermanentAddress = table.Column<string>(nullable: true),
                    Village = table.Column<string>(nullable: true),
                    ProvinceId = table.Column<int>(nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    CountryId = table.Column<int>(nullable: false),
                    RefferedBy = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PassportNumber = table.Column<string>(nullable: true),
                    Nationality = table.Column<string>(nullable: true),
                    SexId = table.Column<int>(nullable: false),
                    DateOfBirth = table.Column<string>(nullable: true),
                    PlaceOfBirth = table.Column<string>(nullable: true),
                    CurrentAddress = table.Column<string>(nullable: true),
                    MaritalStatusId = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    StudentTazkira = table.Column<long>(nullable: false),
                    ParentTazrika = table.Column<long>(nullable: false),
                    PreviousMarksheets = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualDetails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentDetails");

            migrationBuilder.DropTable(
                name: "IndividualDetails");
        }
    }
}
