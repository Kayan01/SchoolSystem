using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class GlobalAdminMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 17, 6, 45, 313, DateTimeKind.Local).AddTicks(9084));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 17, 6, 45, 324, DateTimeKind.Local).AddTicks(6272));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 17, 6, 45, 324, DateTimeKind.Local).AddTicks(7517));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 17, 6, 45, 321, DateTimeKind.Local).AddTicks(2572));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 17, 6, 45, 321, DateTimeKind.Local).AddTicks(5074));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 17, 6, 45, 324, DateTimeKind.Local).AddTicks(3390));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 17, 6, 45, 324, DateTimeKind.Local).AddTicks(4638));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 17, 6, 45, 323, DateTimeKind.Local).AddTicks(7585));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 17, 6, 45, 324, DateTimeKind.Local).AddTicks(1153));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 17, 6, 45, 324, DateTimeKind.Local).AddTicks(1306));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "6801ee8a-7805-41f3-b378-fde145f5998f", new DateTime(2021, 9, 21, 17, 6, 45, 210, DateTimeKind.Local).AddTicks(557), new DateTime(2021, 9, 21, 17, 6, 45, 211, DateTimeKind.Local).AddTicks(3624), "TESTER@GMAIL.COM", "AQAAAAEAACcQAAAAEDqYtsWLLeLJR3stHgqacKdyZaYe/KP2KsxMmGzMWnBprqXqR/Tw5jrI7qS6x7xhjg==", "tester@gmail.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "a59bce42-7b4d-4871-8455-46c122379581", new DateTime(2021, 9, 21, 16, 53, 25, 315, DateTimeKind.Local).AddTicks(2347), new DateTime(2021, 9, 21, 16, 53, 25, 316, DateTimeKind.Local).AddTicks(3946), "TESTER@GMAIL.COM.COM", "AQAAAAEAACcQAAAAEM57A+xbgfZVu3XQ3P0aytBwYhHHXdDcpU8B3Yo+WBxnPkkH6Jv3UbmnCJYa5ObQsQ==", "tester@gmail.com.com" });
        }
    }
}
