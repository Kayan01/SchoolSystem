using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class AUserStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserStatus",
                table: "User",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 12, 30, 41, 703, DateTimeKind.Local).AddTicks(6649));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 12, 30, 41, 712, DateTimeKind.Local).AddTicks(4431));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 12, 30, 41, 712, DateTimeKind.Local).AddTicks(5642));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 12, 30, 41, 709, DateTimeKind.Local).AddTicks(7521));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 12, 30, 41, 710, DateTimeKind.Local).AddTicks(208));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 12, 30, 41, 712, DateTimeKind.Local).AddTicks(2076));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 12, 30, 41, 712, DateTimeKind.Local).AddTicks(3205));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 12, 30, 41, 711, DateTimeKind.Local).AddTicks(9070));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 12, 30, 41, 711, DateTimeKind.Local).AddTicks(9879));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 12, 30, 41, 711, DateTimeKind.Local).AddTicks(9906));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "Email", "LastLoginDate", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "42a5e418-ed28-4851-a743-ad99d71eaed4", new DateTime(2021, 9, 22, 12, 30, 41, 643, DateTimeKind.Local).AddTicks(3418), "root@myschooltrack.com", new DateTime(2021, 9, 22, 12, 30, 41, 645, DateTimeKind.Local).AddTicks(9083), "ROOT@MYSCHOOLTRACK.COM", "ROOT@MYSCHOOLTRACK.COM", "AQAAAAEAACcQAAAAECk9RzjD7I2a4L/KGf9+PYxro2ze5P3W4gDjCWohlrqY26LzJM8204RkjuPtMI2H4w==", "root@myschooltrack.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserStatus",
                table: "User");

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
                columns: new[] { "ConcurrencyStamp", "CreationTime", "Email", "LastLoginDate", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "6801ee8a-7805-41f3-b378-fde145f5998f", new DateTime(2021, 9, 21, 17, 6, 45, 210, DateTimeKind.Local).AddTicks(557), "tester@gmail.com", new DateTime(2021, 9, 21, 12, 29, 21, 644, DateTimeKind.Local).AddTicks(5155), "TESTER@GMAIL.COM", "TESTER@GMAIL.COM", "AQAAAAEAACcQAAAAEDqYtsWLLeLJR3stHgqacKdyZaYe/KP2KsxMmGzMWnBprqXqR/Tw5jrI7qS6x7xhjg==", "tester@gmail.com" });
        }
    }
}
