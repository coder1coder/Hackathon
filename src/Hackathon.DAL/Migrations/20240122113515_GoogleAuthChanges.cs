using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.DAL.Migrations
{
    public partial class GoogleAuthChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "GoogleAccounts");

            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "GoogleAccounts");

            migrationBuilder.DropColumn(
                name: "ExpiresIn",
                table: "GoogleAccounts");

            migrationBuilder.DropColumn(
                name: "FirstIssuedAt",
                table: "GoogleAccounts");

            migrationBuilder.DropColumn(
                name: "GivenName",
                table: "GoogleAccounts");

            migrationBuilder.DropColumn(
                name: "LoginHint",
                table: "GoogleAccounts");

            migrationBuilder.DropColumn(
                name: "TokenId",
                table: "GoogleAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "GoogleAccounts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ExpiresAt",
                table: "GoogleAccounts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ExpiresIn",
                table: "GoogleAccounts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "FirstIssuedAt",
                table: "GoogleAccounts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "GivenName",
                table: "GoogleAccounts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginHint",
                table: "GoogleAccounts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TokenId",
                table: "GoogleAccounts",
                type: "text",
                nullable: true);
        }
    }
}
