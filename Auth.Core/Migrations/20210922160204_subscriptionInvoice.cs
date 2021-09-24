using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class subscriptionInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubscriptionInvoices",
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
                    NumberOfStudent = table.Column<int>(nullable: false),
                    AmountPerStudent = table.Column<int>(nullable: false),
                    InvoiceType = table.Column<int>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    Paid = table.Column<bool>(nullable: false),
                    PaidDate = table.Column<DateTime>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionInvoices_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 19, DateTimeKind.Local).AddTicks(3332));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 24, DateTimeKind.Local).AddTicks(5247));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 24, DateTimeKind.Local).AddTicks(5678));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 23, DateTimeKind.Local).AddTicks(2854));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 23, DateTimeKind.Local).AddTicks(3856));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 24, DateTimeKind.Local).AddTicks(3836));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 24, DateTimeKind.Local).AddTicks(4401));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 24, DateTimeKind.Local).AddTicks(2656));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 24, DateTimeKind.Local).AddTicks(2990));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 22, 17, 2, 4, 24, DateTimeKind.Local).AddTicks(3003));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "1e4ac682-2d6c-453e-aa1b-e1ec2498036a", new DateTime(2021, 9, 22, 17, 2, 3, 989, DateTimeKind.Local).AddTicks(5795), new DateTime(2021, 9, 22, 17, 2, 3, 990, DateTimeKind.Local).AddTicks(7345), "AQAAAAEAACcQAAAAEH08y08qvQKomKUn4L/5/YnEvnghGQ698L4iG8D+5vKiqqqt3vKgwzbpyy9ae5DxQg==" });

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionInvoices_SchoolId",
                table: "SubscriptionInvoices",
                column: "SchoolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriptionInvoices");

            migrationBuilder.DropColumn(
                name: "UserStatus",
                table: "User");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 15, 37, 42, 149, DateTimeKind.Local).AddTicks(7893));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 15, 37, 42, 154, DateTimeKind.Local).AddTicks(1208));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 15, 37, 42, 154, DateTimeKind.Local).AddTicks(1714));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 15, 37, 42, 153, DateTimeKind.Local).AddTicks(891));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 15, 37, 42, 153, DateTimeKind.Local).AddTicks(1754));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 15, 37, 42, 154, DateTimeKind.Local).AddTicks(211));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 15, 37, 42, 154, DateTimeKind.Local).AddTicks(586));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 15, 37, 42, 153, DateTimeKind.Local).AddTicks(9245));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 15, 37, 42, 153, DateTimeKind.Local).AddTicks(9567));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 9, 21, 15, 37, 42, 153, DateTimeKind.Local).AddTicks(9580));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "02f092ca-3afe-4dad-aec1-d54362f4380a", new DateTime(2021, 9, 21, 15, 37, 42, 124, DateTimeKind.Local).AddTicks(3113), new DateTime(2021, 9, 21, 15, 37, 42, 125, DateTimeKind.Local).AddTicks(5872), "AQAAAAEAACcQAAAAEHMjEVc4lKbRk3hPHjjuGbKOovGOvlI219esHRuCDLnuOR3dgrgnI87TYtOcpycu5g==" });
        }
    }
}
