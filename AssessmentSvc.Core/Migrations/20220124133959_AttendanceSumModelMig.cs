using Microsoft.EntityFrameworkCore.Migrations;

namespace AssessmentSvc.Core.Migrations
{
    public partial class AttendanceSumModelMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassAttendance_SchoolClasses_ClassId",
                table: "ClassAttendance");

            migrationBuilder.DropIndex(
                name: "IX_ClassAttendance_ClassId",
                table: "ClassAttendance");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ClassAttendance_ClassId",
                table: "ClassAttendance",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassAttendance_SchoolClasses_ClassId",
                table: "ClassAttendance",
                column: "ClassId",
                principalTable: "SchoolClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
