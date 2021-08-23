using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class updatesgg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTerminalClass",
                table: "Classes",
                nullable: false,
                defaultValue: false);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTerminalClass",
                table: "Classes");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 224, DateTimeKind.Local).AddTicks(6071));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 241, DateTimeKind.Local).AddTicks(1355));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 241, DateTimeKind.Local).AddTicks(4074));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 236, DateTimeKind.Local).AddTicks(1522));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 236, DateTimeKind.Local).AddTicks(6725));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 239, DateTimeKind.Local).AddTicks(8375));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 240, DateTimeKind.Local).AddTicks(1215));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 239, DateTimeKind.Local).AddTicks(2342));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 239, DateTimeKind.Local).AddTicks(5049));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 239, DateTimeKind.Local).AddTicks(5111));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "996f632e-9d8f-49c6-8b92-3a2603dcc4ff", new DateTime(2021, 8, 23, 14, 18, 55, 113, DateTimeKind.Local).AddTicks(8137), new DateTime(2021, 8, 23, 14, 18, 55, 118, DateTimeKind.Local).AddTicks(190), "AQAAAAEAACcQAAAAEMBi/PedZWyhx7bPMKjqirC6cEeKsb0YM/sa51FcV+YmeVX/JNXEfFBbDjIsr3CtWg==" });
        }
    }
}
