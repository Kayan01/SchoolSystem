using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class merge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 9, 54, 18, 761, DateTimeKind.Local).AddTicks(9849));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 9, 54, 18, 774, DateTimeKind.Local).AddTicks(2785));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 9, 54, 18, 774, DateTimeKind.Local).AddTicks(3917));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 9, 54, 18, 769, DateTimeKind.Local).AddTicks(8624));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 9, 54, 18, 770, DateTimeKind.Local).AddTicks(1423));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 9, 54, 18, 773, DateTimeKind.Local).AddTicks(9907));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 9, 54, 18, 774, DateTimeKind.Local).AddTicks(1176));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 9, 54, 18, 773, DateTimeKind.Local).AddTicks(5299));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 9, 54, 18, 773, DateTimeKind.Local).AddTicks(6573));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 13, 9, 54, 18, 773, DateTimeKind.Local).AddTicks(6629));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "e8a3d2b0-f91b-44ad-9850-8423caeed983", new DateTime(2021, 9, 13, 9, 54, 18, 687, DateTimeKind.Local).AddTicks(8386), new DateTime(2021, 9, 13, 9, 54, 18, 690, DateTimeKind.Local).AddTicks(7274), "AQAAAAEAACcQAAAAEFN3FILWN0CtqxbiueL5N2uZiinvGgMurSxss/KWAYHKRbKbUSI919lnehIJ3ZY1Fg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
