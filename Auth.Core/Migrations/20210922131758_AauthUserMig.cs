using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class AauthUserMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 14, 17, 56, 308, DateTimeKind.Local).AddTicks(6322));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 14, 17, 56, 320, DateTimeKind.Local).AddTicks(829));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 14, 17, 56, 320, DateTimeKind.Local).AddTicks(4643));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 14, 17, 56, 315, DateTimeKind.Local).AddTicks(6904));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 14, 17, 56, 316, DateTimeKind.Local).AddTicks(353));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 14, 17, 56, 319, DateTimeKind.Local).AddTicks(7996));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 14, 17, 56, 319, DateTimeKind.Local).AddTicks(9524));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 14, 17, 56, 319, DateTimeKind.Local).AddTicks(5542));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 14, 17, 56, 319, DateTimeKind.Local).AddTicks(6311));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 14, 17, 56, 319, DateTimeKind.Local).AddTicks(6337));

            //migrationBuilder.UpdateData(
            //    table: "User",
            //    keyColumn: "Id",
            //    keyValue: 1L,
            //    columns: new[] { "ConcurrencyStamp", "CreationTime", "Email", "LastLoginDate", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
            //    values: new object[] { "4a4ab6ce-af35-402c-9c4f-df722bf6747f", new DateTime(2021, 9, 22, 14, 17, 56, 237, DateTimeKind.Local).AddTicks(1409), "tester@gmail.com", new DateTime(2021, 9, 22, 14, 17, 56, 238, DateTimeKind.Local).AddTicks(2031), "TESTER@GMAIL.COM", "TESTER@GMAIL.COM", "AQAAAAEAACcQAAAAEIMhCg2xd1hSURzWyw1lQckL0dDl0mU9YAEdNt88mm4q2zyVMFvh+e3xIFih8RfM2w==", "tester@gmail.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            //migrationBuilder.UpdateData(
            //    table: "User",
            //    keyColumn: "Id",
            //    keyValue: 1L,
            //    columns: new[] { "ConcurrencyStamp", "CreationTime", "Email", "LastLoginDate", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
            //    values: new object[] { "42a5e418-ed28-4851-a743-ad99d71eaed4", new DateTime(2021, 9, 22, 12, 30, 41, 643, DateTimeKind.Local).AddTicks(3418), "root@myschooltrack.com", new DateTime(2021, 9, 22, 12, 30, 41, 645, DateTimeKind.Local).AddTicks(9083), "ROOT@MYSCHOOLTRACK.COM", "ROOT@MYSCHOOLTRACK.COM", "AQAAAAEAACcQAAAAECk9RzjD7I2a4L/KGf9+PYxro2ze5P3W4gDjCWohlrqY26LzJM8204RkjuPtMI2H4w==", "root@myschooltrack.com" });
        }
    }
}
