using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class firsttimelogin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                columns: new[] { "ConcurrencyStamp", "CreationTime", "IsFirstTimeLogin", "LastLoginDate", "PasswordHash" },
                values: new object[] { "bd13b271-08bd-46e8-ba07-1c4c0abbbcb8", new DateTime(2021, 7, 6, 9, 55, 35, 194, DateTimeKind.Local).AddTicks(3085), false, new DateTime(2021, 7, 6, 9, 55, 35, 195, DateTimeKind.Local).AddTicks(7207), "AQAAAAEAACcQAAAAEIIer7RxCL68C0pO8q7QaeX7SfSjWjXndB/4IlkoSHjPIiuOLFdvu/iaYkrX5Yub3A==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 45, 4, 167, DateTimeKind.Local).AddTicks(7983));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 45, 4, 181, DateTimeKind.Local).AddTicks(666));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 45, 4, 181, DateTimeKind.Local).AddTicks(2519));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 45, 4, 178, DateTimeKind.Local).AddTicks(1755));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 45, 4, 178, DateTimeKind.Local).AddTicks(6041));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 45, 4, 180, DateTimeKind.Local).AddTicks(6995));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 45, 4, 180, DateTimeKind.Local).AddTicks(8911));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 45, 4, 180, DateTimeKind.Local).AddTicks(3499));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 45, 4, 180, DateTimeKind.Local).AddTicks(4919));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 7, 6, 9, 45, 4, 180, DateTimeKind.Local).AddTicks(4949));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "IsFirstTimeLogin", "LastLoginDate", "PasswordHash" },
                values: new object[] { "a5632271-7847-4b8f-a698-cac4f86cbff8", new DateTime(2021, 7, 6, 9, 45, 4, 88, DateTimeKind.Local).AddTicks(4450), true, new DateTime(2021, 7, 6, 9, 45, 4, 90, DateTimeKind.Local).AddTicks(4391), "AQAAAAEAACcQAAAAEGAd9Wl2/qLtXyWh19fBT87sl2+MiGrqatgtY96umC6lGetLItHc0Z4U0MqtXprsZg==" });
        }
    }
}
