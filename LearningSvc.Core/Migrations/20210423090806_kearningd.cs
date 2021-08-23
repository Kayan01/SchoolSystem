using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LearningSvc.Core.Migrations
{
    public partial class kearningd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Students_Parent_ParentId",
            //    table: "Students");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Parent",
            //    table: "Parent");

            //migrationBuilder.RenameTable(
            //    name: "Parent",
            //    newName: "Parents");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Parents",
            //    table: "Parents",
            //    column: "Id");

            ////migrationBuilder.UpdateData(
            ////    table: "MyNotice",
            ////    keyColumn: "Id",
            ////    keyValue: 1L,
            ////    column: "CreationTime",
            ////    value: new DateTime(2021, 4, 23, 10, 8, 6, 392, DateTimeKind.Local).AddTicks(2147));

            ////migrationBuilder.UpdateData(
            ////    table: "MyNotice",
            ////    keyColumn: "Id",
            ////    keyValue: 2L,
            ////    column: "CreationTime",
            ////    value: new DateTime(2021, 4, 23, 10, 8, 6, 392, DateTimeKind.Local).AddTicks(4052));

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Students_Parents_ParentId",
            //    table: "Students",
            //    column: "ParentId",
            //    principalTable: "Parents",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Parents_ParentId",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parents",
                table: "Parents");

            migrationBuilder.RenameTable(
                name: "Parents",
                newName: "Parent");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parent",
                table: "Parent",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "MyNotice",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 22, 17, 53, 19, 13, DateTimeKind.Local).AddTicks(8628));

            migrationBuilder.UpdateData(
                table: "MyNotice",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 22, 17, 53, 19, 14, DateTimeKind.Local).AddTicks(1064));

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Parent_ParentId",
                table: "Students",
                column: "ParentId",
                principalTable: "Parent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
