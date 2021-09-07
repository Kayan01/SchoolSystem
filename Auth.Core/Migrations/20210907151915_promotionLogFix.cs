using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class promotionLogFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromotionLogs_Classes_SchoolClassId",
                table: "PromotionLogs");

            migrationBuilder.DropIndex(
                name: "IX_PromotionLogs_SchoolClassId",
                table: "PromotionLogs");

            migrationBuilder.DropColumn(
                name: "SchoolClassId",
                table: "PromotionLogs");

            migrationBuilder.AddColumn<string>(
                name: "ClassPoolName",
                table: "PromotionLogs",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FromClassId",
                table: "PromotionLogs",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ToClassId",
                table: "PromotionLogs",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_PromotionLogs_FromClassId",
                table: "PromotionLogs",
                column: "FromClassId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionLogs_ToClassId",
                table: "PromotionLogs",
                column: "ToClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_PromotionLogs_Classes_FromClassId",
                table: "PromotionLogs",
                column: "FromClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PromotionLogs_Classes_ToClassId",
                table: "PromotionLogs",
                column: "ToClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromotionLogs_Classes_FromClassId",
                table: "PromotionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_PromotionLogs_Classes_ToClassId",
                table: "PromotionLogs");

            migrationBuilder.DropIndex(
                name: "IX_PromotionLogs_FromClassId",
                table: "PromotionLogs");

            migrationBuilder.DropIndex(
                name: "IX_PromotionLogs_ToClassId",
                table: "PromotionLogs");

            migrationBuilder.DropColumn(
                name: "ClassPoolName",
                table: "PromotionLogs");

            migrationBuilder.DropColumn(
                name: "FromClassId",
                table: "PromotionLogs");

            migrationBuilder.DropColumn(
                name: "ToClassId",
                table: "PromotionLogs");

            migrationBuilder.AddColumn<long>(
                name: "SchoolClassId",
                table: "PromotionLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 10, 37, 10, 689, DateTimeKind.Local).AddTicks(4589));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 10, 37, 10, 694, DateTimeKind.Local).AddTicks(9727));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 10, 37, 10, 695, DateTimeKind.Local).AddTicks(306));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 10, 37, 10, 693, DateTimeKind.Local).AddTicks(5391));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 10, 37, 10, 693, DateTimeKind.Local).AddTicks(6755));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 10, 37, 10, 694, DateTimeKind.Local).AddTicks(8362));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 10, 37, 10, 694, DateTimeKind.Local).AddTicks(8976));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 10, 37, 10, 694, DateTimeKind.Local).AddTicks(6885));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 10, 37, 10, 694, DateTimeKind.Local).AddTicks(7356));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 7, 10, 37, 10, 694, DateTimeKind.Local).AddTicks(7374));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "048047ac-7e94-4f14-a2ce-553513e2323f", new DateTime(2021, 9, 7, 10, 37, 10, 651, DateTimeKind.Local).AddTicks(5953), new DateTime(2021, 9, 7, 10, 37, 10, 652, DateTimeKind.Local).AddTicks(7369), "AQAAAAEAACcQAAAAEDyhYid/GD1MdHe22J0z5m+HrOwPozt0578Iv3e4ZkIIwsT6SCOgJ1uOBwMU/N1ldw==" });

            migrationBuilder.CreateIndex(
                name: "IX_PromotionLogs_SchoolClassId",
                table: "PromotionLogs",
                column: "SchoolClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_PromotionLogs_Classes_SchoolClassId",
                table: "PromotionLogs",
                column: "SchoolClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
