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
                    StudentId = table.Column<long>(nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 11, 26, 13, 63, DateTimeKind.Local).AddTicks(7557));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 11, 26, 13, 75, DateTimeKind.Local).AddTicks(1429));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 11, 26, 13, 75, DateTimeKind.Local).AddTicks(2687));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 11, 26, 13, 72, DateTimeKind.Local).AddTicks(2951));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 11, 26, 13, 72, DateTimeKind.Local).AddTicks(6195));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 11, 26, 13, 74, DateTimeKind.Local).AddTicks(8832));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 11, 26, 13, 74, DateTimeKind.Local).AddTicks(9926));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 11, 26, 13, 74, DateTimeKind.Local).AddTicks(5560));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 11, 26, 13, 74, DateTimeKind.Local).AddTicks(6633));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 11, 26, 13, 74, DateTimeKind.Local).AddTicks(6671));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "3e8e0d0f-d8f9-4375-a880-8aa99eff111f", new DateTime(2021, 9, 3, 11, 26, 12, 993, DateTimeKind.Local).AddTicks(8850), new DateTime(2021, 9, 3, 11, 26, 12, 996, DateTimeKind.Local).AddTicks(7000), "AQAAAAEAACcQAAAAEAiDQ9O6Hy50Cnyu6gMXpUFTTfPbNM2m6k6gFvz8RHH0UfPqFkAiHSUX3+mt17Poew==" });

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
