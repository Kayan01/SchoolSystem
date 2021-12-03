using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LearningSvc.Core.Migrations
{
    public partial class EmailPasswordMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailPassword",
                table: "Schools",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "MyNotice",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 12, 3, 13, 35, 57, 557, DateTimeKind.Local).AddTicks(8050));

            migrationBuilder.UpdateData(
                table: "MyNotice",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 12, 3, 13, 35, 57, 557, DateTimeKind.Local).AddTicks(9952));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailPassword",
                table: "Schools");

            migrationBuilder.UpdateData(
                table: "MyNotice",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 10, 6, 16, 18, 17, 604, DateTimeKind.Local).AddTicks(7422));

            migrationBuilder.UpdateData(
                table: "MyNotice",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 10, 6, 16, 18, 17, 604, DateTimeKind.Local).AddTicks(8188));
        }
    }
}
