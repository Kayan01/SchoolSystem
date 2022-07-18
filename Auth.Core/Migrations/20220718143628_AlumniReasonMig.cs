using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class AlumniReasonMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AlumniReason",
                table: "Alumnis",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2022, 7, 18, 15, 36, 27, 591, DateTimeKind.Local).AddTicks(9873));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2022, 7, 18, 15, 36, 27, 595, DateTimeKind.Local).AddTicks(9915));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2022, 7, 18, 15, 36, 27, 596, DateTimeKind.Local).AddTicks(324));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2022, 7, 18, 15, 36, 27, 594, DateTimeKind.Local).AddTicks(9140));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2022, 7, 18, 15, 36, 27, 595, DateTimeKind.Local).AddTicks(24));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2022, 7, 18, 15, 36, 27, 595, DateTimeKind.Local).AddTicks(9095));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2022, 7, 18, 15, 36, 27, 595, DateTimeKind.Local).AddTicks(9472));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2022, 7, 18, 15, 36, 27, 595, DateTimeKind.Local).AddTicks(8106));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2022, 7, 18, 15, 36, 27, 595, DateTimeKind.Local).AddTicks(8353));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2022, 7, 18, 15, 36, 27, 595, DateTimeKind.Local).AddTicks(8365));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "312a2996-7214-4be7-8faf-461b9a3d4939", new DateTime(2022, 7, 18, 15, 36, 27, 569, DateTimeKind.Local).AddTicks(394), new DateTime(2022, 7, 18, 15, 36, 27, 569, DateTimeKind.Local).AddTicks(7669), "AQAAAAEAACcQAAAAEM2AAhBfPmYQ4CyJ3cvkRs2XdZnVxDXuS9c7Qd08g1MhggD5CgmQvTZ73NRBiridWw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlumniReason",
                table: "Alumnis");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 12, 8, 16, 53, 1, 381, DateTimeKind.Local).AddTicks(6011));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 12, 8, 16, 53, 1, 398, DateTimeKind.Local).AddTicks(5668));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 12, 8, 16, 53, 1, 398, DateTimeKind.Local).AddTicks(7534));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 12, 8, 16, 53, 1, 393, DateTimeKind.Local).AddTicks(3627));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 12, 8, 16, 53, 1, 393, DateTimeKind.Local).AddTicks(7797));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 12, 8, 16, 53, 1, 398, DateTimeKind.Local).AddTicks(1874));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 12, 8, 16, 53, 1, 398, DateTimeKind.Local).AddTicks(3608));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 12, 8, 16, 53, 1, 397, DateTimeKind.Local).AddTicks(8037));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 12, 8, 16, 53, 1, 397, DateTimeKind.Local).AddTicks(9167));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 12, 8, 16, 53, 1, 397, DateTimeKind.Local).AddTicks(9212));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "7d48b5af-5f10-447a-8cad-2a3000b34a02", new DateTime(2021, 12, 8, 16, 53, 1, 185, DateTimeKind.Local).AddTicks(3720), new DateTime(2021, 12, 8, 16, 53, 1, 186, DateTimeKind.Local).AddTicks(5083), "AQAAAAEAACcQAAAAEOmID07+VzVduNS+ZpISj05F4D8clHaXYMytLW5Uvw/4XqquL4o8Vsguy69XMX6/zw==" });
        }
    }
}
