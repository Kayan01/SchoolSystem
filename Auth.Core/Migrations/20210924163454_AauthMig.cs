using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class AauthMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 694, DateTimeKind.Local).AddTicks(7040));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 711, DateTimeKind.Local).AddTicks(811));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 711, DateTimeKind.Local).AddTicks(2291));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 705, DateTimeKind.Local).AddTicks(9050));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 706, DateTimeKind.Local).AddTicks(2412));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 710, DateTimeKind.Local).AddTicks(7400));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 710, DateTimeKind.Local).AddTicks(9026));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 710, DateTimeKind.Local).AddTicks(3812));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 710, DateTimeKind.Local).AddTicks(4901));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 710, DateTimeKind.Local).AddTicks(4967));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "Email", "LastLoginDate", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "975293fa-09a7-430f-ae95-333927fe734c", new DateTime(2021, 9, 24, 17, 34, 26, 593, DateTimeKind.Local).AddTicks(4556), "tester@gmail.com", new DateTime(2021, 9, 24, 17, 34, 26, 594, DateTimeKind.Local).AddTicks(7795), "TESTER@GMAIL.COM", "TESTER@GMAIL.COM", "AQAAAAEAACcQAAAAEJeJFvKk/p/g7A0ih06yUjoFbzd5EEhz5KyS5Z7gIIBRu4YwUzbFWw7TE0AuWAjAWQ==", "tester@gmail.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 19, DateTimeKind.Local).AddTicks(3332));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 24, DateTimeKind.Local).AddTicks(5247));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 24, DateTimeKind.Local).AddTicks(5678));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 23, DateTimeKind.Local).AddTicks(2854));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 23, DateTimeKind.Local).AddTicks(3856));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 24, DateTimeKind.Local).AddTicks(3836));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 24, DateTimeKind.Local).AddTicks(4401));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 24, DateTimeKind.Local).AddTicks(2656));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 24, DateTimeKind.Local).AddTicks(2990));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 24, DateTimeKind.Local).AddTicks(3003));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "Email", "LastLoginDate", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "1e4ac682-2d6c-453e-aa1b-e1ec2498036a", new DateTime(2021, 9, 22, 17, 2, 3, 989, DateTimeKind.Local).AddTicks(5795), "root@myschooltrack.com", new DateTime(2021, 9, 22, 17, 2, 3, 990, DateTimeKind.Local).AddTicks(7345), "ROOT@MYSCHOOLTRACK.COM", "ROOT@MYSCHOOLTRACK.COM", "AQAAAAEAACcQAAAAEH08y08qvQKomKUn4L/5/YnEvnghGQ698L4iG8D+5vKiqqqt3vKgwzbpyy9ae5DxQg==", "root@myschooltrack.com" });
        }
    }
}
