using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class contactdetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolContactDetails_Schools_SchoolId",
                table: "SchoolContactDetails");

            migrationBuilder.AlterColumn<long>(
                name: "SchoolId",
                table: "SchoolContactDetails",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 1, DateTimeKind.Local).AddTicks(5031));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 13, DateTimeKind.Local).AddTicks(1080));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 13, DateTimeKind.Local).AddTicks(2434));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 10, DateTimeKind.Local).AddTicks(8197));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 11, DateTimeKind.Local).AddTicks(1497));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 12, DateTimeKind.Local).AddTicks(8329));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 12, DateTimeKind.Local).AddTicks(9772));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 12, DateTimeKind.Local).AddTicks(5303));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 12, DateTimeKind.Local).AddTicks(6190));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 12, DateTimeKind.Local).AddTicks(6213));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "272d9ecb-c65b-4df8-9e6a-0fcb65a1656a", new DateTime(2021, 4, 28, 18, 17, 36, 934, DateTimeKind.Local).AddTicks(3021), new DateTime(2021, 4, 28, 18, 17, 36, 936, DateTimeKind.Local).AddTicks(1707), "AQAAAAEAACcQAAAAEBcRjy643zXrLB95A5peeqmdPqDjO9OCYBTm0Z0zJDgs4UyTfdoYjeVhO/1FWyFZ6Q==" });

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolContactDetails_Schools_SchoolId",
                table: "SchoolContactDetails",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolContactDetails_Schools_SchoolId",
                table: "SchoolContactDetails");

            migrationBuilder.AlterColumn<long>(
                name: "SchoolId",
                table: "SchoolContactDetails",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 21, 13, 48, 37, 699, DateTimeKind.Local).AddTicks(8261));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 21, 13, 48, 37, 709, DateTimeKind.Local).AddTicks(6523));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 21, 13, 48, 37, 709, DateTimeKind.Local).AddTicks(8032));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 4, 21, 13, 48, 37, 707, DateTimeKind.Local).AddTicks(33));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 4, 21, 13, 48, 37, 707, DateTimeKind.Local).AddTicks(4273));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 21, 13, 48, 37, 709, DateTimeKind.Local).AddTicks(3727));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 21, 13, 48, 37, 709, DateTimeKind.Local).AddTicks(5107));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 21, 13, 48, 37, 709, DateTimeKind.Local).AddTicks(1112));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 21, 13, 48, 37, 709, DateTimeKind.Local).AddTicks(2005));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 21, 13, 48, 37, 709, DateTimeKind.Local).AddTicks(2024));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "bf3f3c6f-eb08-4f58-8453-5fa8736ffa45", new DateTime(2021, 4, 21, 13, 48, 37, 628, DateTimeKind.Local).AddTicks(809), new DateTime(2021, 4, 21, 13, 48, 37, 629, DateTimeKind.Local).AddTicks(5316), "AQAAAAEAACcQAAAAELZrRJE3NQyvkklPZZ29NxuSz/XbBi8rY+5LbHJF9hNyp+0eOBdDrVxLo8GWk2cgLQ==" });

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolContactDetails_Schools_SchoolId",
                table: "SchoolContactDetails",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
