using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class nullSchoolIdMig : Migration
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
                value: new DateTime(2021, 12, 6, 14, 52, 57, 224, DateTimeKind.Local).AddTicks(5622));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 12, 6, 14, 52, 57, 237, DateTimeKind.Local).AddTicks(3894));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 12, 6, 14, 52, 57, 238, DateTimeKind.Local).AddTicks(9654));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 12, 6, 14, 52, 57, 232, DateTimeKind.Local).AddTicks(2970));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 12, 6, 14, 52, 57, 232, DateTimeKind.Local).AddTicks(8255));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 12, 6, 14, 52, 57, 237, DateTimeKind.Local).AddTicks(392));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 12, 6, 14, 52, 57, 237, DateTimeKind.Local).AddTicks(2041));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 12, 6, 14, 52, 57, 236, DateTimeKind.Local).AddTicks(6827));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 12, 6, 14, 52, 57, 236, DateTimeKind.Local).AddTicks(7851));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 12, 6, 14, 52, 57, 236, DateTimeKind.Local).AddTicks(7903));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "b5ec81aa-0b38-4610-a618-b099b1ee4a37", new DateTime(2021, 12, 6, 14, 52, 57, 117, DateTimeKind.Local).AddTicks(8485), new DateTime(2021, 12, 6, 14, 52, 57, 119, DateTimeKind.Local).AddTicks(1451), "AQAAAAEAACcQAAAAEODiPPnn1G2aR4kS2KVWhwLww0nndr6EtLaOmG9TlKCBM8/9Ai+13e6QdolhkUcU/Q==" });

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
