using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class addingschoolId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolContactDetails_Schools_SchoolId",
                table: "SchoolContactDetails");

            migrationBuilder.AlterColumn<long>(
                name: "SchoolId",
                table: "SchoolContactDetails",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 11, 23, 11, 53, 4, 344, DateTimeKind.Local).AddTicks(8378));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 11, 23, 11, 53, 4, 355, DateTimeKind.Local).AddTicks(268));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 11, 23, 11, 53, 4, 355, DateTimeKind.Local).AddTicks(2739));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 11, 23, 11, 53, 4, 351, DateTimeKind.Local).AddTicks(2175));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 11, 23, 11, 53, 4, 351, DateTimeKind.Local).AddTicks(4536));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 11, 23, 11, 53, 4, 354, DateTimeKind.Local).AddTicks(5774));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 11, 23, 11, 53, 4, 354, DateTimeKind.Local).AddTicks(7515));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 11, 23, 11, 53, 4, 353, DateTimeKind.Local).AddTicks(5399));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 11, 23, 11, 53, 4, 354, DateTimeKind.Local).AddTicks(2950));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 11, 23, 11, 53, 4, 354, DateTimeKind.Local).AddTicks(3017));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "Email", "LastLoginDate", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "fa300de1-f6ac-4dd6-b0ca-e93b079f736c", new DateTime(2021, 11, 23, 11, 53, 4, 285, DateTimeKind.Local).AddTicks(2847), "root@myschooltrack.com", new DateTime(2021, 11, 23, 11, 53, 4, 285, DateTimeKind.Local).AddTicks(9782), "ROOT@MYSCHOOLTRACK.COM", "ROOT@MYSCHOOLTRACK.COM", "AQAAAAEAACcQAAAAELBqmARIFyfzZTjnGgv6+HZJvCCcF0sUOEtY/lkp0iwKDa+08lCISSeAE1TKcwcRhQ==", "root@myschooltrack.com" });

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolContactDetails_Schools_SchoolId",
                table: "SchoolContactDetails",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 694, DateTimeKind.Local).AddTicks(7040));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 711, DateTimeKind.Local).AddTicks(811));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 711, DateTimeKind.Local).AddTicks(2291));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 705, DateTimeKind.Local).AddTicks(9050));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 706, DateTimeKind.Local).AddTicks(2412));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 710, DateTimeKind.Local).AddTicks(7400));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 710, DateTimeKind.Local).AddTicks(9026));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 710, DateTimeKind.Local).AddTicks(3812));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 710, DateTimeKind.Local).AddTicks(4901));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 24, 17, 34, 26, 710, DateTimeKind.Local).AddTicks(4967));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "Email", "LastLoginDate", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "975293fa-09a7-430f-ae95-333927fe734c", new DateTime(2021, 9, 24, 17, 34, 26, 593, DateTimeKind.Local).AddTicks(4556), "tester@gmail.com", new DateTime(2021, 9, 24, 17, 34, 26, 594, DateTimeKind.Local).AddTicks(7795), "TESTER@GMAIL.COM", "TESTER@GMAIL.COM", "AQAAAAEAACcQAAAAEJeJFvKk/p/g7A0ih06yUjoFbzd5EEhz5KyS5Z7gIIBRu4YwUzbFWw7TE0AuWAjAWQ==", "tester@gmail.com" });

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolContactDetails_Schools_SchoolId",
                table: "SchoolContactDetails",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
