using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicDistory.Data.Migrations
{
    public partial class AddedAuditColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditType",
                table: "UserAudits");

            migrationBuilder.AddColumn<int>(
                name: "ActionType",
                table: "UserAudits",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "UserAudits",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EventDescription",
                table: "UserAudits",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionType",
                table: "UserAudits");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "UserAudits");

            migrationBuilder.DropColumn(
                name: "EventDescription",
                table: "UserAudits");

            migrationBuilder.AddColumn<int>(
                name: "AuditType",
                table: "UserAudits",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
