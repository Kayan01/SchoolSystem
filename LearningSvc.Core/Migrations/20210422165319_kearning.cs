using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LearningSvc.Core.Migrations
{
    public partial class kearning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<long>(
            //    name: "ParentId",
            //    table: "Students",
            //    nullable: false,
            //    defaultValue: 0L);

            //migrationBuilder.CreateTable(
            //    name: "Parent",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(nullable: false),
            //        CreationTime = table.Column<DateTime>(nullable: false),
            //        CreatorUserId = table.Column<long>(nullable: true),
            //        LastModificationTime = table.Column<DateTime>(nullable: true),
            //        LastModifierUserId = table.Column<long>(nullable: true),
            //        UserId = table.Column<long>(nullable: false),
            //        IsDeleted = table.Column<bool>(nullable: false),
            //        IsActive = table.Column<bool>(nullable: false),
            //        FirstName = table.Column<string>(nullable: true),
            //        LastName = table.Column<string>(nullable: true),
            //        Email = table.Column<string>(nullable: true),
            //        Phone = table.Column<string>(nullable: true),
            //        RegNumber = table.Column<string>(nullable: true),
            //        SecondaryPhoneNumber = table.Column<string>(nullable: true),
            //        SecondaryEmail = table.Column<string>(nullable: true),
            //        HomeAddress = table.Column<string>(nullable: true),
            //        OfficeAddress = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Parent", x => x.Id);
            //    });

            //migrationBuilder.UpdateData(
            //    table: "MyNotice",
            //    keyColumn: "Id",
            //    keyValue: 1L,
            //    column: "CreationTime",
            //    value: new DateTime(2021, 4, 22, 17, 53, 19, 13, DateTimeKind.Local).AddTicks(8628));

            //migrationBuilder.UpdateData(
            //    table: "MyNotice",
            //    keyColumn: "Id",
            //    keyValue: 2L,
            //    column: "CreationTime",
            //    value: new DateTime(2021, 4, 22, 17, 53, 19, 14, DateTimeKind.Local).AddTicks(1064));

            //migrationBuilder.CreateIndex(
            //    name: "IX_Students_ParentId",
            //    table: "Students",
            //    column: "ParentId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Students_Parent_ParentId",
            //    table: "Students",
            //    column: "ParentId",
            //    principalTable: "Parent",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Parent_ParentId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Parent");

            migrationBuilder.DropIndex(
                name: "IX_Students_ParentId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Students");

            migrationBuilder.UpdateData(
                table: "MyNotice",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 20, 10, 0, 10, 105, DateTimeKind.Local).AddTicks(7021));

            migrationBuilder.UpdateData(
                table: "MyNotice",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 20, 10, 0, 10, 105, DateTimeKind.Local).AddTicks(9655));
        }
    }
}
