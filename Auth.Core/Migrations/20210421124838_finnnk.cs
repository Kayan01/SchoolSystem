using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class finnnk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DomainName",
                table: "Schools",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
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

            migrationBuilder.CreateIndex(
                name: "IX_Schools_DomainName",
                table: "Schools",
                column: "DomainName",
                unique: true,
                filter: "[DomainName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schools_DomainName",
                table: "Schools");

            migrationBuilder.AlterColumn<string>(
                name: "DomainName",
                table: "Schools",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 20, 10, 3, 35, 899, DateTimeKind.Local).AddTicks(5618));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 20, 10, 3, 35, 913, DateTimeKind.Local).AddTicks(8900));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 20, 10, 3, 35, 914, DateTimeKind.Local).AddTicks(1044));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 4, 20, 10, 3, 35, 910, DateTimeKind.Local).AddTicks(4854));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 4, 20, 10, 3, 35, 911, DateTimeKind.Local).AddTicks(20));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 20, 10, 3, 35, 913, DateTimeKind.Local).AddTicks(4643));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 20, 10, 3, 35, 913, DateTimeKind.Local).AddTicks(6953));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 20, 10, 3, 35, 913, DateTimeKind.Local).AddTicks(824));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 20, 10, 3, 35, 913, DateTimeKind.Local).AddTicks(2093));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 20, 10, 3, 35, 913, DateTimeKind.Local).AddTicks(2131));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "77e6f3cc-e2a2-4db6-b1f2-237b22d0364e", new DateTime(2021, 4, 20, 10, 3, 35, 822, DateTimeKind.Local).AddTicks(7580), new DateTime(2021, 4, 20, 10, 3, 35, 824, DateTimeKind.Local).AddTicks(2469), "AQAAAAEAACcQAAAAEGjLehT4Hl5wMeC+2CxPkoxeK/evG73Dn6EEnDhvpuAle1ZezfF1sdDq9Apb6Uk3fQ==" });
        }
    }
}
