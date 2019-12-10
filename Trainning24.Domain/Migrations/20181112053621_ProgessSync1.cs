using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class ProgessSync1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "TotalRecords",
                table: "progessSync",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "QuizId",
                table: "progessSync",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "LessionProgessId",
                table: "progessSync",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<decimal>(
                name: "LessionProgess",
                table: "progessSync",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<long>(
                name: "LessionId",
                table: "progessSync",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "GradeId",
                table: "progessSync",
                nullable: true,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "TotalRecords",
                table: "progessSync",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "QuizId",
                table: "progessSync",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "LessionProgessId",
                table: "progessSync",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LessionProgess",
                table: "progessSync",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "LessionId",
                table: "progessSync",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "GradeId",
                table: "progessSync",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
