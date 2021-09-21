using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FacilitySvc.Core.Migrations
{
    public partial class FacilityMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileUploads",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUploads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MyNotice",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyNotice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PublishedMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Topic = table.Column<string>(nullable: true),
                    MessageType = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishedMessage", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MyNotice",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "Description", "LastModificationTime", "LastModifierUserId" },
                values: new object[] { 1L, new DateTime(2021, 9, 21, 12, 36, 42, 105, DateTimeKind.Local).AddTicks(1795), null, "Testing", null, null });

            migrationBuilder.InsertData(
                table: "MyNotice",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "Description", "LastModificationTime", "LastModifierUserId" },
                values: new object[] { 2L, new DateTime(2021, 9, 21, 12, 36, 42, 105, DateTimeKind.Local).AddTicks(4800), null, "Unit Test", null, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileUploads");

            migrationBuilder.DropTable(
                name: "MyNotice");

            migrationBuilder.DropTable(
                name: "PublishedMessage");
        }
    }
}
