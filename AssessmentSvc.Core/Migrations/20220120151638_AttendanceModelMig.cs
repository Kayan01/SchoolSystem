using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssessmentSvc.Core.Migrations
{
    public partial class AttendanceModelMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassAttendance",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<long>(nullable: false),
                    AttendanceDate = table.Column<DateTime>(nullable: false),
                    StudentId = table.Column<long>(nullable: false),
                    AttendanceStatus = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    ClassId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassAttendance_SchoolClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "SchoolClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassAttendance_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectAttendance",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<long>(nullable: false),
                    AttendanceDate = table.Column<DateTime>(nullable: false),
                    AttendanceStatus = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    SubjectId = table.Column<long>(nullable: false),
                    StudentId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectAttendance_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectAttendance_SchoolClassSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "SchoolClassSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassAttendance_ClassId",
                table: "ClassAttendance",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassAttendance_StudentId",
                table: "ClassAttendance",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAttendance_StudentId",
                table: "SubjectAttendance",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAttendance_SubjectId",
                table: "SubjectAttendance",
                column: "SubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassAttendance");

            migrationBuilder.DropTable(
                name: "SubjectAttendance");
        }
    }
}
