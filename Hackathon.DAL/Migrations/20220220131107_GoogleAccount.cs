using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.DAL.Migrations
{
    public partial class GoogleAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleAccountId",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GoogleAccounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    GiveName = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    AccessToken = table.Column<string>(type: "text", nullable: true),
                    ExpiresAt = table.Column<long>(type: "bigint", nullable: false),
                    ExpiresIn = table.Column<long>(type: "bigint", nullable: false),
                    FirstIssuedAt = table.Column<long>(type: "bigint", nullable: false),
                    TokenId = table.Column<string>(type: "text", nullable: true),
                    LoginHint = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleAccounts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_GoogleAccountId",
                table: "Users",
                column: "GoogleAccountId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_GoogleAccounts_GoogleAccountId",
                table: "Users",
                column: "GoogleAccountId",
                principalTable: "GoogleAccounts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_GoogleAccounts_GoogleAccountId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "GoogleAccounts");

            migrationBuilder.DropIndex(
                name: "IX_Users_GoogleAccountId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GoogleAccountId",
                table: "Users");
        }
    }
}
