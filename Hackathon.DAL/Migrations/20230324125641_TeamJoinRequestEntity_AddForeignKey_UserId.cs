using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.DAL.Migrations
{
    public partial class TeamJoinRequestEntity_AddForeignKey_UserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TeamJoinRequests_UserId",
                table: "TeamJoinRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamJoinRequests_Users_UserId",
                table: "TeamJoinRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamJoinRequests_Users_UserId",
                table: "TeamJoinRequests");

            migrationBuilder.DropIndex(
                name: "IX_TeamJoinRequests_UserId",
                table: "TeamJoinRequests");
        }
    }
}
