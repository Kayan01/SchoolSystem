using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssessmentSvc.Core.Migrations
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

            migrationBuilder.CreateTable(
                name: "PromotionSetups",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<long>(nullable: false),
                    PromotionMethod = table.Column<int>(nullable: false),
                    PromotionType = table.Column<int>(nullable: false),
                    PromotionScore = table.Column<int>(nullable: false),
                    MaxRepeat = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionSetups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultSummaries",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<long>(nullable: false),
                    StudentId = table.Column<long>(nullable: false),
                    SessionSetupId = table.Column<long>(nullable: false),
                    TermSequenceNumber = table.Column<int>(nullable: false),
                    SchoolClassId = table.Column<long>(nullable: false),
                    ResultTotalAverage = table.Column<double>(nullable: false),
                    ResultTotal = table.Column<double>(nullable: false),
                    ResultApproved = table.Column<bool>(nullable: false),
                    ClassPosition = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultSummaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultSummaries_SchoolClasses_SchoolClassId",
                        column: x => x.SchoolClassId,
                        principalTable: "SchoolClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResultSummaries_SessionSetups_SessionSetupId",
                        column: x => x.SessionSetupId,
                        principalTable: "SessionSetups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResultSummaries_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolClassSubjects",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<long>(nullable: false),
                    SchoolClassId = table.Column<long>(nullable: false),
                    SubjectId = table.Column<long>(nullable: false),
                    StudentId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolClassSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolClassSubjects_SchoolClasses_SchoolClassId",
                        column: x => x.SchoolClassId,
                        principalTable: "SchoolClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolClassSubjects_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolClassSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolPromotionLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<long>(nullable: false),
                    SessionSetupId = table.Column<long>(nullable: false),
                    Payload = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolPromotionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolPromotionLogs_SessionSetups_SessionSetupId",
                        column: x => x.SessionSetupId,
                        principalTable: "SessionSetups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResultSummaries_SchoolClassId",
                table: "ResultSummaries",
                column: "SchoolClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultSummaries_SessionSetupId",
                table: "ResultSummaries",
                column: "SessionSetupId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultSummaries_StudentId",
                table: "ResultSummaries",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolClassSubjects_SchoolClassId",
                table: "SchoolClassSubjects",
                column: "SchoolClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolClassSubjects_StudentId",
                table: "SchoolClassSubjects",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolClassSubjects_SubjectId",
                table: "SchoolClassSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolPromotionLogs_SessionSetupId",
                table: "SchoolPromotionLogs",
                column: "SessionSetupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromotionSetups");

            migrationBuilder.DropTable(
                name: "ResultSummaries");

            migrationBuilder.DropTable(
                name: "SchoolClassSubjects");

            migrationBuilder.DropTable(
                name: "SchoolPromotionLogs");

            migrationBuilder.DropColumn(
                name: "StudentStatusInSchool",
                table: "Students");
        }
    }
}
