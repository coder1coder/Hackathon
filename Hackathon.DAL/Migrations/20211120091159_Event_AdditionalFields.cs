using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hackathon.DAL.Migrations
{
    public partial class Event_AdditionalFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxEventMembers",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinTeamMembers",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartMemberRegistration",
                table: "Events",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxEventMembers",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MinTeamMembers",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "StartMemberRegistration",
                table: "Events");
        }
    }
}
