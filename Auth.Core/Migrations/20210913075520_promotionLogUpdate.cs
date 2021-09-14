using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class promotionLogUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "PromotionLogs");

            migrationBuilder.AddColumn<string>(
                name: "ReInstateReason",
                table: "PromotionLogs",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionName",
                table: "PromotionLogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WithdrawalReason",
                table: "PromotionLogs",
                maxLength: 300,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 8, 55, 19, 185, DateTimeKind.Local).AddTicks(5521));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 8, 55, 19, 192, DateTimeKind.Local).AddTicks(5940));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 8, 55, 19, 192, DateTimeKind.Local).AddTicks(6497));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 8, 55, 19, 190, DateTimeKind.Local).AddTicks(9928));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 8, 55, 19, 191, DateTimeKind.Local).AddTicks(1815));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 8, 55, 19, 192, DateTimeKind.Local).AddTicks(4355));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 8, 55, 19, 192, DateTimeKind.Local).AddTicks(5048));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 8, 55, 19, 192, DateTimeKind.Local).AddTicks(2670));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 8, 55, 19, 192, DateTimeKind.Local).AddTicks(3091));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 8, 55, 19, 192, DateTimeKind.Local).AddTicks(3114));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "06fe67a8-c253-441b-a918-107cf0fa50ab", new DateTime(2021, 9, 13, 8, 55, 19, 143, DateTimeKind.Local).AddTicks(8403), new DateTime(2021, 9, 13, 8, 55, 19, 145, DateTimeKind.Local).AddTicks(4193), "AQAAAAEAACcQAAAAEMfyB2jrhkiN8V9YcdG6hvteoL9cQTOKsCdC1b7A+AyQK0fQXAGBWm/tuMAUwvYFfg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReInstateReason",
                table: "PromotionLogs");

            migrationBuilder.DropColumn(
                name: "SessionName",
                table: "PromotionLogs");

            migrationBuilder.DropColumn(
                name: "WithdrawalReason",
                table: "PromotionLogs");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "PromotionLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 725, DateTimeKind.Local).AddTicks(7692));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 735, DateTimeKind.Local).AddTicks(7604));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 735, DateTimeKind.Local).AddTicks(8172));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 733, DateTimeKind.Local).AddTicks(3622));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 733, DateTimeKind.Local).AddTicks(7416));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 735, DateTimeKind.Local).AddTicks(6142));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 735, DateTimeKind.Local).AddTicks(6733));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 735, DateTimeKind.Local).AddTicks(4335));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 735, DateTimeKind.Local).AddTicks(4857));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 735, DateTimeKind.Local).AddTicks(4876));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "99f24736-0a7b-4d08-ac5d-5af0210834a1", new DateTime(2021, 9, 7, 16, 19, 14, 680, DateTimeKind.Local).AddTicks(7840), new DateTime(2021, 9, 7, 16, 19, 14, 681, DateTimeKind.Local).AddTicks(8868), "AQAAAAEAACcQAAAAEF74n6CnlLux/xfTzLAjCaVKVOK8rAdLfimi1G1OJqJBkVoC1ggIQmEP8Pu2WSg7Yw==" });
        }
    }
}
