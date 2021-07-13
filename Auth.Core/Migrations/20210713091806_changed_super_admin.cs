using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class changed_super_admin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 13, 10, 18, 4, 893, DateTimeKind.Local).AddTicks(7034));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 13, 10, 18, 4, 902, DateTimeKind.Local).AddTicks(4297));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 13, 10, 18, 4, 902, DateTimeKind.Local).AddTicks(5374));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 7, 13, 10, 18, 4, 899, DateTimeKind.Local).AddTicks(6820));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 7, 13, 10, 18, 4, 899, DateTimeKind.Local).AddTicks(9044));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 13, 10, 18, 4, 902, DateTimeKind.Local).AddTicks(1334));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 13, 10, 18, 4, 902, DateTimeKind.Local).AddTicks(2813));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 13, 10, 18, 4, 901, DateTimeKind.Local).AddTicks(6896));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 13, 10, 18, 4, 901, DateTimeKind.Local).AddTicks(8856));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 13, 10, 18, 4, 901, DateTimeKind.Local).AddTicks(8930));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "Email", "FirstName", "LastLoginDate", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "4f645828-96e9-4bdd-8db7-feb5f25a3f2f", new DateTime(2021, 7, 13, 10, 18, 4, 818, DateTimeKind.Local).AddTicks(374), "root@myschooltrack.com", "Super Admin", new DateTime(2021, 7, 13, 10, 18, 4, 822, DateTimeKind.Local).AddTicks(2586), "ROOT@MYSCHOOLTRACK.COM", "ROOT@MYSCHOOLTRACK.COM", "AQAAAAEAACcQAAAAELKosIo7lN0uLYXbQemOivgP1sk3B7XqBGFLlvHwzTIQ3VRnmdnOiTpoNndxO8bwvg==", "root@myschooltrack.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 55, 35, 252, DateTimeKind.Local).AddTicks(5572));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 55, 35, 260, DateTimeKind.Local).AddTicks(4626));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 55, 35, 260, DateTimeKind.Local).AddTicks(5711));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 55, 35, 258, DateTimeKind.Local).AddTicks(5069));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 55, 35, 258, DateTimeKind.Local).AddTicks(8622));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 55, 35, 260, DateTimeKind.Local).AddTicks(2445));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 55, 35, 260, DateTimeKind.Local).AddTicks(3585));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 55, 35, 260, DateTimeKind.Local).AddTicks(262));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 55, 35, 260, DateTimeKind.Local).AddTicks(1010));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 55, 35, 260, DateTimeKind.Local).AddTicks(1028));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "Email", "FirstName", "LastLoginDate", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "bd13b271-08bd-46e8-ba07-1c4c0abbbcb8", new DateTime(2021, 7, 6, 9, 55, 35, 194, DateTimeKind.Local).AddTicks(3085), "tester@gmail.com", "Tester", new DateTime(2021, 7, 6, 9, 55, 35, 195, DateTimeKind.Local).AddTicks(7207), "TESTER@GMAIL.COM", "TESTER@GMAIL.COM", "AQAAAAEAACcQAAAAEIIer7RxCL68C0pO8q7QaeX7SfSjWjXndB/4IlkoSHjPIiuOLFdvu/iaYkrX5Yub3A==", "tester@gmail.com" });
        }
    }
}
