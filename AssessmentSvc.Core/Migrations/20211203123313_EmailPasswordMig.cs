using Microsoft.EntityFrameworkCore.Migrations;

namespace AssessmentSvc.Core.Migrations
{
    public partial class EmailPasswordMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailPassword",
                table: "Schools",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailPassword",
                table: "Schools");
        }
    }
}
