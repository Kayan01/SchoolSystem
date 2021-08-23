using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NotificationSvc.Core.Migrations
{
    public partial class notifi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Sent = table.Column<bool>(nullable: false),
                    JsonReplacements = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EntityId = table.Column<long>(nullable: true),
                    Entity = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "TestModels",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserNotification",
                columns: table => new
                {
                    NotificationId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false),
                    DateRead = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotification", x => new { x.NotificationId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserNotification_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Notification",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "Description", "Entity", "EntityId" },
                values: new object[] { 1L, new DateTime(2020, 12, 9, 14, 29, 54, 315, DateTimeKind.Local).AddTicks(1675), null, "Testing", null, null });

            migrationBuilder.InsertData(
                table: "Notification",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "Description", "Entity", "EntityId" },
                values: new object[] { 2L, new DateTime(2020, 12, 9, 14, 29, 54, 315, DateTimeKind.Local).AddTicks(5341), null, "Unit Test", null, null });

            migrationBuilder.InsertData(
                table: "UserNotification",
                columns: new[] { "NotificationId", "UserId", "DateRead", "IsRead" },
                values: new object[] { 1L, 1L, new DateTime(2020, 12, 9, 14, 29, 54, 321, DateTimeKind.Local).AddTicks(5560), true });

            migrationBuilder.InsertData(
                table: "UserNotification",
                columns: new[] { "NotificationId", "UserId", "DateRead", "IsRead" },
                values: new object[] { 2L, 1L, null, false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DropTable(
                name: "PublishedMessage");

            migrationBuilder.DropTable(
                name: "TestModels");

            migrationBuilder.DropTable(
                name: "UserNotification");

            migrationBuilder.DropTable(
                name: "Notification");
        }
    }
}
