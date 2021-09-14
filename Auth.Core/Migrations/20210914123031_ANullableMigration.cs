using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class ANullableMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ParentId",
                table: "Alumnis",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 14, 13, 30, 30, 450, DateTimeKind.Local).AddTicks(3394));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 14, 13, 30, 30, 459, DateTimeKind.Local).AddTicks(270));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 14, 13, 30, 30, 459, DateTimeKind.Local).AddTicks(1351));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 14, 13, 30, 30, 456, DateTimeKind.Local).AddTicks(3695));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 14, 13, 30, 30, 456, DateTimeKind.Local).AddTicks(6032));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 14, 13, 30, 30, 458, DateTimeKind.Local).AddTicks(7900));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 14, 13, 30, 30, 458, DateTimeKind.Local).AddTicks(9022));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 14, 13, 30, 30, 458, DateTimeKind.Local).AddTicks(5196));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 14, 13, 30, 30, 458, DateTimeKind.Local).AddTicks(5962));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 14, 13, 30, 30, 458, DateTimeKind.Local).AddTicks(5987));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "53bea828-a2fc-4004-a9bb-d088f67ac3db", new DateTime(2021, 9, 14, 13, 30, 30, 400, DateTimeKind.Local).AddTicks(3245), new DateTime(2021, 9, 14, 13, 30, 30, 400, DateTimeKind.Local).AddTicks(9969), "AQAAAAEAACcQAAAAEFXF8EH6mSXoaEARuEta0S7DFSks2YFmwmwoY6UINjLDF2oQljRqlHMPRLosqf+WgA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ParentId",
                table: "Alumnis",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 8, 9, 45, 21, 212, DateTimeKind.Local).AddTicks(2591));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 8, 9, 45, 21, 221, DateTimeKind.Local).AddTicks(7384));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 8, 9, 45, 21, 221, DateTimeKind.Local).AddTicks(8517));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 8, 9, 45, 21, 219, DateTimeKind.Local).AddTicks(2691));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 8, 9, 45, 21, 219, DateTimeKind.Local).AddTicks(4883));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 8, 9, 45, 21, 221, DateTimeKind.Local).AddTicks(5188));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 8, 9, 45, 21, 221, DateTimeKind.Local).AddTicks(6195));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 8, 9, 45, 21, 221, DateTimeKind.Local).AddTicks(2917));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 8, 9, 45, 21, 221, DateTimeKind.Local).AddTicks(3609));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 8, 9, 45, 21, 221, DateTimeKind.Local).AddTicks(3637));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "5523dba3-504f-4e36-a3df-1c4b993e7aa2", new DateTime(2021, 9, 8, 9, 45, 21, 157, DateTimeKind.Local).AddTicks(7490), new DateTime(2021, 9, 8, 9, 45, 21, 158, DateTimeKind.Local).AddTicks(9319), "AQAAAAEAACcQAAAAEIpFdWh9oM8aOwLYzFBV8mDU6zb97R+qvxZCTmCQWz3QUlVvvZ4ZO8NNxZKk84qkoQ==" });
        }
    }
}
