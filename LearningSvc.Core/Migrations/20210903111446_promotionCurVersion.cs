using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LearningSvc.Core.Migrations
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

            migrationBuilder.UpdateData(
                table: "MyNotice",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 14, 46, 484, DateTimeKind.Local).AddTicks(2002));

            migrationBuilder.UpdateData(
                table: "MyNotice",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 14, 46, 484, DateTimeKind.Local).AddTicks(2803));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentStatusInSchool",
                table: "Students");

            migrationBuilder.UpdateData(
                table: "MyNotice",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 26, 15, 18, 19, 724, DateTimeKind.Local).AddTicks(1456));

            migrationBuilder.UpdateData(
                table: "MyNotice",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 26, 15, 18, 19, 724, DateTimeKind.Local).AddTicks(5086));
        }
    }
}
