using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class GlobalAdminMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 16, 53, 25, 416, DateTimeKind.Local).AddTicks(8027));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 16, 53, 25, 432, DateTimeKind.Local).AddTicks(751));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 16, 53, 25, 432, DateTimeKind.Local).AddTicks(2160));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 16, 53, 25, 427, DateTimeKind.Local).AddTicks(6462));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 16, 53, 25, 427, DateTimeKind.Local).AddTicks(9870));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 16, 53, 25, 431, DateTimeKind.Local).AddTicks(7316));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 16, 53, 25, 431, DateTimeKind.Local).AddTicks(8846));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 16, 53, 25, 431, DateTimeKind.Local).AddTicks(3547));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 16, 53, 25, 431, DateTimeKind.Local).AddTicks(4696));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 16, 53, 25, 431, DateTimeKind.Local).AddTicks(4740));

            //migrationBuilder.UpdateData(
            //    table: "User",
            //    keyColumn: "Id",
            //    keyValue: 1L,
            //    columns: new[] { "ConcurrencyStamp", "CreationTime", "Email", "LastLoginDate", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
            //    values: new object[] { "a59bce42-7b4d-4871-8455-46c122379581", new DateTime(2021, 9, 21, 16, 53, 25, 315, DateTimeKind.Local).AddTicks(2347), "tester@gmail.com", new DateTime(2021, 9, 21, 16, 53, 25, 316, DateTimeKind.Local).AddTicks(3946), "TESTER@GMAIL.COM", "TESTER@GMAIL.COM.COM", "AQAAAAEAACcQAAAAEM57A+xbgfZVu3XQ3P0aytBwYhHHXdDcpU8B3Yo+WBxnPkkH6Jv3UbmnCJYa5ObQsQ==", "tester@gmail.com.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 12, 29, 21, 738, DateTimeKind.Local).AddTicks(977));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 12, 29, 21, 750, DateTimeKind.Local).AddTicks(2582));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 12, 29, 21, 750, DateTimeKind.Local).AddTicks(3941));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 12, 29, 21, 746, DateTimeKind.Local).AddTicks(2009));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 12, 29, 21, 746, DateTimeKind.Local).AddTicks(5323));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 12, 29, 21, 749, DateTimeKind.Local).AddTicks(9732));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 12, 29, 21, 750, DateTimeKind.Local).AddTicks(971));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 12, 29, 21, 749, DateTimeKind.Local).AddTicks(6892));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 12, 29, 21, 749, DateTimeKind.Local).AddTicks(7766));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 12, 29, 21, 749, DateTimeKind.Local).AddTicks(7805));

            //migrationBuilder.UpdateData(
            //    table: "User",
            //    keyColumn: "Id",
            //    keyValue: 1L,
            //    columns: new[] { "ConcurrencyStamp", "CreationTime", "Email", "LastLoginDate", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
            //    values: new object[] { "5469b22a-2448-412d-a386-22578a19662e", new DateTime(2021, 9, 21, 12, 29, 21, 643, DateTimeKind.Local).AddTicks(2144), "root@myschooltrack.com", new DateTime(2021, 9, 21, 12, 29, 21, 644, DateTimeKind.Local).AddTicks(5155), "ROOT@MYSCHOOLTRACK.COM", "ROOT@MYSCHOOLTRACK.COM", "AQAAAAEAACcQAAAAEPy9SX1yKkeykACCYFXEP5egYywlIp/s6Qs17zhl3M1BYrSwY3VRTVCk1qCaFiU1+Q==", "root@myschooltrack.com" });
        }
    }
}
