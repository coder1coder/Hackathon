using System.Collections.Generic;
using Hackathon.Common.Models.Event;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.DAL.Migrations
{
    public partial class ChangeColumnNameGoogle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GiveName",
                table: "GoogleAccounts",
                newName: "GivenName");

            migrationBuilder.AlterColumn<ICollection<ChangeEventStatusMessage>>(
                name: "ChangeEventStatusMessages",
                table: "Events",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(ICollection<ChangeEventStatusMessage>),
                oldType: "jsonb",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GivenName",
                table: "GoogleAccounts",
                newName: "GiveName");

            migrationBuilder.AlterColumn<ICollection<ChangeEventStatusMessage>>(
                name: "ChangeEventStatusMessages",
                table: "Events",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(ICollection<ChangeEventStatusMessage>),
                oldType: "jsonb");
        }
    }
}
