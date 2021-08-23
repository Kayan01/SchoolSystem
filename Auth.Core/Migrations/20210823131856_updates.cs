using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SchoolGroupId",
                table: "Schools",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SchoolGroupId",
                table: "SchoolContactDetails",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AlumniId",
                table: "FileUploads",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SchoolGroupId",
                table: "FileUploads",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Alumnis",
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
                    UserId = table.Column<long>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Sex = table.Column<string>(nullable: true),
                    RegNumber = table.Column<string>(nullable: true),
                    StudentId = table.Column<long>(nullable: false),
                    MothersMaidenName = table.Column<string>(nullable: true),
                    Nationality = table.Column<string>(nullable: true),
                    Religion = table.Column<string>(nullable: true),
                    StateOfOrigin = table.Column<string>(nullable: true),
                    LocalGovernment = table.Column<string>(nullable: true),
                    TransportRoute = table.Column<string>(nullable: true),
                    AdmissionDate = table.Column<DateTime>(nullable: false),
                    StudentType = table.Column<int>(nullable: false),
                    Level = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Town = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    ParentId = table.Column<long>(nullable: false),
                    MedicalDetailID = table.Column<long>(nullable: false),
                    SessionName = table.Column<string>(nullable: true),
                    TermName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumnis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alumnis_Schools_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alumnis_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolGroups",
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
                    Name = table.Column<string>(nullable: true),
                    WebsiteAddress = table.Column<string>(nullable: true),
                    PrimaryColor = table.Column<string>(nullable: true),
                    SecondaryColor = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolGroups", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 224, DateTimeKind.Local).AddTicks(6071));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 241, DateTimeKind.Local).AddTicks(1355));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 241, DateTimeKind.Local).AddTicks(4074));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 236, DateTimeKind.Local).AddTicks(1522));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 236, DateTimeKind.Local).AddTicks(6725));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 239, DateTimeKind.Local).AddTicks(8375));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 240, DateTimeKind.Local).AddTicks(1215));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 239, DateTimeKind.Local).AddTicks(2342));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 239, DateTimeKind.Local).AddTicks(5049));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 8, 23, 14, 18, 55, 239, DateTimeKind.Local).AddTicks(5111));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "996f632e-9d8f-49c6-8b92-3a2603dcc4ff", new DateTime(2021, 8, 23, 14, 18, 55, 113, DateTimeKind.Local).AddTicks(8137), new DateTime(2021, 8, 23, 14, 18, 55, 118, DateTimeKind.Local).AddTicks(190), "AQAAAAEAACcQAAAAEMBi/PedZWyhx7bPMKjqirC6cEeKsb0YM/sa51FcV+YmeVX/JNXEfFBbDjIsr3CtWg==" });

            migrationBuilder.CreateIndex(
                name: "IX_Schools_SchoolGroupId",
                table: "Schools",
                column: "SchoolGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolContactDetails_SchoolGroupId",
                table: "SchoolContactDetails",
                column: "SchoolGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUploads_AlumniId",
                table: "FileUploads",
                column: "AlumniId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUploads_SchoolGroupId",
                table: "FileUploads",
                column: "SchoolGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumnis_TenantId",
                table: "Alumnis",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumnis_UserId",
                table: "Alumnis",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileUploads_Alumnis_AlumniId",
                table: "FileUploads",
                column: "AlumniId",
                principalTable: "Alumnis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FileUploads_SchoolGroups_SchoolGroupId",
                table: "FileUploads",
                column: "SchoolGroupId",
                principalTable: "SchoolGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolContactDetails_SchoolGroups_SchoolGroupId",
                table: "SchoolContactDetails",
                column: "SchoolGroupId",
                principalTable: "SchoolGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_SchoolGroups_SchoolGroupId",
                table: "Schools",
                column: "SchoolGroupId",
                principalTable: "SchoolGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileUploads_Alumnis_AlumniId",
                table: "FileUploads");

            migrationBuilder.DropForeignKey(
                name: "FK_FileUploads_SchoolGroups_SchoolGroupId",
                table: "FileUploads");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolContactDetails_SchoolGroups_SchoolGroupId",
                table: "SchoolContactDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_SchoolGroups_SchoolGroupId",
                table: "Schools");

            migrationBuilder.DropTable(
                name: "Alumnis");

            migrationBuilder.DropTable(
                name: "SchoolGroups");

            migrationBuilder.DropIndex(
                name: "IX_Schools_SchoolGroupId",
                table: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_SchoolContactDetails_SchoolGroupId",
                table: "SchoolContactDetails");

            migrationBuilder.DropIndex(
                name: "IX_FileUploads_AlumniId",
                table: "FileUploads");

            migrationBuilder.DropIndex(
                name: "IX_FileUploads_SchoolGroupId",
                table: "FileUploads");

            migrationBuilder.DropColumn(
                name: "SchoolGroupId",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "SchoolGroupId",
                table: "SchoolContactDetails");

            migrationBuilder.DropColumn(
                name: "AlumniId",
                table: "FileUploads");

            migrationBuilder.DropColumn(
                name: "SchoolGroupId",
                table: "FileUploads");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 1, DateTimeKind.Local).AddTicks(5031));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 13, DateTimeKind.Local).AddTicks(1080));

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 13, DateTimeKind.Local).AddTicks(2434));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 10, DateTimeKind.Local).AddTicks(8197));

            migrationBuilder.UpdateData(
                table: "MyTest",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 11, DateTimeKind.Local).AddTicks(1497));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 12, DateTimeKind.Local).AddTicks(8329));

            migrationBuilder.UpdateData(
                table: "SchoolSections",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 12, DateTimeKind.Local).AddTicks(9772));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 12, DateTimeKind.Local).AddTicks(5303));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 12, DateTimeKind.Local).AddTicks(6190));

            migrationBuilder.UpdateData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationTime",
                value: new DateTime(2021, 4, 28, 18, 17, 37, 12, DateTimeKind.Local).AddTicks(6213));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "CreationTime", "LastLoginDate", "PasswordHash" },
                values: new object[] { "272d9ecb-c65b-4df8-9e6a-0fcb65a1656a", new DateTime(2021, 4, 28, 18, 17, 36, 934, DateTimeKind.Local).AddTicks(3021), new DateTime(2021, 4, 28, 18, 17, 36, 936, DateTimeKind.Local).AddTicks(1707), "AQAAAAEAACcQAAAAEBcRjy643zXrLB95A5peeqmdPqDjO9OCYBTm0Z0zJDgs4UyTfdoYjeVhO/1FWyFZ6Q==" });
        }
    }
}
