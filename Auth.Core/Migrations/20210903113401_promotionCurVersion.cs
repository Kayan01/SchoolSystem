using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
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
                name: "PromotionLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<long>(nullable: false),
                    PromotionStatus = table.Column<int>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    StudentId = table.Column<long>(nullable: true),
                    SessionSetupId = table.Column<long>(nullable: false),
                    SchoolClassId = table.Column<long>(nullable: false),
                    AverageScore = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionLogs_Classes_SchoolClassId",
                        column: x => x.SchoolClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionLogs_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 109, DateTimeKind.Local).AddTicks(6721));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 114, DateTimeKind.Local).AddTicks(6009));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 114, DateTimeKind.Local).AddTicks(6416));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 113, DateTimeKind.Local).AddTicks(5023));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 113, DateTimeKind.Local).AddTicks(5930));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 114, DateTimeKind.Local).AddTicks(5021));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 114, DateTimeKind.Local).AddTicks(5438));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 114, DateTimeKind.Local).AddTicks(4022));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 114, DateTimeKind.Local).AddTicks(4349));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 114, DateTimeKind.Local).AddTicks(4361));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "b9824a6b-308b-4c15-b629-246c63ac870c", new DateTime(2021, 9, 3, 12, 34, 0, 81, DateTimeKind.Local).AddTicks(880), new DateTime(2021, 9, 3, 12, 34, 0, 82, DateTimeKind.Local).AddTicks(424), "AQAAAAEAACcQAAAAEObpIFFhWPSbg1xjAfB1ceRCcL2ayd5emj5mjbluLHB7OetNCKI0RYPwXXdOGPmPBw==" });

            migrationBuilder.CreateIndex(
                name: "IX_PromotionLogs_SchoolClassId",
                table: "PromotionLogs",
                column: "SchoolClassId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionLogs_StudentId",
                table: "PromotionLogs",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromotionLogs");

            migrationBuilder.DropColumn(
                name: "StudentStatusInSchool",
                table: "Students");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 58, 35, 786, DateTimeKind.Local).AddTicks(9366));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 58, 35, 801, DateTimeKind.Local).AddTicks(2771));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 58, 35, 801, DateTimeKind.Local).AddTicks(5099));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 58, 35, 797, DateTimeKind.Local).AddTicks(1151));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 58, 35, 797, DateTimeKind.Local).AddTicks(5651));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 58, 35, 800, DateTimeKind.Local).AddTicks(7884));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 58, 35, 801, DateTimeKind.Local).AddTicks(228));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 58, 35, 800, DateTimeKind.Local).AddTicks(2749));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 58, 35, 800, DateTimeKind.Local).AddTicks(4748));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 58, 35, 800, DateTimeKind.Local).AddTicks(4802));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "7e8d35b9-9e7f-4132-bb62-37a28ba0b687", new DateTime(2021, 8, 23, 14, 58, 35, 700, DateTimeKind.Local).AddTicks(7159), new DateTime(2021, 8, 23, 14, 58, 35, 705, DateTimeKind.Local).AddTicks(8951), "AQAAAAEAACcQAAAAEJiUEITSv3vOdjj8jdoPov1CC8v/+1o2z2ZRvG8uyRvhCvSmQKocVkh1zxCK0RdWyw==" });
        }
    }
}
