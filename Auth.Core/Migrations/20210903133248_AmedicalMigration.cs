using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class AmedicalMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_MedicalDetail_MedicalDetailID",
                table: "Students");

            migrationBuilder.AlterColumn<long>(
                name: "ParentId",
                table: "Students",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "MedicalDetailID",
                table: "Students",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 14, 32, 47, 406, DateTimeKind.Local).AddTicks(3100));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 14, 32, 47, 413, DateTimeKind.Local).AddTicks(8039));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 14, 32, 47, 413, DateTimeKind.Local).AddTicks(9157));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 14, 32, 47, 411, DateTimeKind.Local).AddTicks(8637));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 14, 32, 47, 412, DateTimeKind.Local).AddTicks(825));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 14, 32, 47, 413, DateTimeKind.Local).AddTicks(5911));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 14, 32, 47, 413, DateTimeKind.Local).AddTicks(6929));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 14, 32, 47, 413, DateTimeKind.Local).AddTicks(4029));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 14, 32, 47, 413, DateTimeKind.Local).AddTicks(4671));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 14, 32, 47, 413, DateTimeKind.Local).AddTicks(4696));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "8b0afa78-0ff5-41f8-a4fb-65cdea0e14e5", new DateTime(2021, 9, 3, 14, 32, 47, 341, DateTimeKind.Local).AddTicks(3164), new DateTime(2021, 9, 3, 14, 32, 47, 342, DateTimeKind.Local).AddTicks(3895), "AQAAAAEAACcQAAAAEKh7zyv87oELty3tCHHRT8LcHOPXrQu0WCL2ZyLyI5TcAOukbDHV+mD64VAitun8Bw==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Students_MedicalDetail_MedicalDetailID",
                table: "Students",
                column: "MedicalDetailID",
                principalTable: "MedicalDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_MedicalDetail_MedicalDetailID",
                table: "Students");

            migrationBuilder.AlterColumn<long>(
                name: "ParentId",
                table: "Students",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "MedicalDetailID",
                table: "Students",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 109, DateTimeKind.Local).AddTicks(6721));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 114, DateTimeKind.Local).AddTicks(6009));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 114, DateTimeKind.Local).AddTicks(6416));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 113, DateTimeKind.Local).AddTicks(5023));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 113, DateTimeKind.Local).AddTicks(5930));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 114, DateTimeKind.Local).AddTicks(5021));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 114, DateTimeKind.Local).AddTicks(5438));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 114, DateTimeKind.Local).AddTicks(4022));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 114, DateTimeKind.Local).AddTicks(4349));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 3, 12, 34, 0, 114, DateTimeKind.Local).AddTicks(4361));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "b9824a6b-308b-4c15-b629-246c63ac870c", new DateTime(2021, 9, 3, 12, 34, 0, 81, DateTimeKind.Local).AddTicks(880), new DateTime(2021, 9, 3, 12, 34, 0, 82, DateTimeKind.Local).AddTicks(424), "AQAAAAEAACcQAAAAEObpIFFhWPSbg1xjAfB1ceRCcL2ayd5emj5mjbluLHB7OetNCKI0RYPwXXdOGPmPBw==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Students_MedicalDetail_MedicalDetailID",
                table: "Students",
                column: "MedicalDetailID",
                principalTable: "MedicalDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
