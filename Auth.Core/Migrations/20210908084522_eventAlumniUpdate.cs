using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class eventAlumniUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EventImageId",
                table: "AlumniEvents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "AlumniEvents",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_AlumniEvents_EventImageId",
                table: "AlumniEvents",
                column: "EventImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlumniEvents_FileUploads_EventImageId",
                table: "AlumniEvents",
                column: "EventImageId",
                principalTable: "FileUploads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlumniEvents_FileUploads_EventImageId",
                table: "AlumniEvents");

            migrationBuilder.DropIndex(
                name: "IX_AlumniEvents_EventImageId",
                table: "AlumniEvents");

            migrationBuilder.DropColumn(
                name: "EventImageId",
                table: "AlumniEvents");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "AlumniEvents");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 725, DateTimeKind.Local).AddTicks(7692));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 735, DateTimeKind.Local).AddTicks(7604));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 735, DateTimeKind.Local).AddTicks(8172));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 733, DateTimeKind.Local).AddTicks(3622));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 733, DateTimeKind.Local).AddTicks(7416));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 735, DateTimeKind.Local).AddTicks(6142));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 735, DateTimeKind.Local).AddTicks(6733));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 735, DateTimeKind.Local).AddTicks(4335));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 735, DateTimeKind.Local).AddTicks(4857));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 16, 19, 14, 735, DateTimeKind.Local).AddTicks(4876));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "99f24736-0a7b-4d08-ac5d-5af0210834a1", new DateTime(2021, 9, 7, 16, 19, 14, 680, DateTimeKind.Local).AddTicks(7840), new DateTime(2021, 9, 7, 16, 19, 14, 681, DateTimeKind.Local).AddTicks(8868), "AQAAAAEAACcQAAAAEF74n6CnlLux/xfTzLAjCaVKVOK8rAdLfimi1G1OJqJBkVoC1ggIQmEP8Pu2WSg7Yw==" });
        }
    }
}
