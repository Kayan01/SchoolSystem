using Microsoft.EntityFrameworkCore.Migrations;

namespace AssessmentSvc.Core.Migrations
{
    public partial class ass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExam",
                table: "AssessmentSetups",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExam",
                table: "AssessmentSetups");
        }
    }
}
