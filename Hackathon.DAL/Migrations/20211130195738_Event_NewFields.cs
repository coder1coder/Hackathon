using System;
using System.Collections.Generic;
using Hackathon.Common.Models.Event;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hackathon.DAL.Migrations
{
    public partial class Event_NewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartMemberRegistration",
                table: "Events");

            migrationBuilder.AddColumn<List<ChangeEventStatusMessage>>(
                name: "ChangeEventStatusMessages",
                table: "Events",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DevelopmentMinutes",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCreateTeamsAutomatically",
                table: "Events",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TeamPresentationMinutes",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangeEventStatusMessages",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "DevelopmentMinutes",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsCreateTeamsAutomatically",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TeamPresentationMinutes",
                table: "Events");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartMemberRegistration",
                table: "Events",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
