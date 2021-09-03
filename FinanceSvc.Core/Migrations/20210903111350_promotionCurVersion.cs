using Microsoft.EntityFrameworkCore.Migrations;

namespace FinanceSvc.Core.Migrations
{
    public partial class promotionCurVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentStatusInSchool",
                table: "Students",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentStatusInSchool",
                table: "Students");
        }
    }
}
