using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class eventAlumni : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlumniEvents",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlumniEvents", x => x.Id);
                });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlumniEvents");

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
        }
    }
}
