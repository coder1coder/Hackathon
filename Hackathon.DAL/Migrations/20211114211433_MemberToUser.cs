using Microsoft.EntityFrameworkCore.Migrations;

namespace Hackathon.DAL.Migrations
{
    public partial class MemberToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamEntityUserEntity");

            migrationBuilder.CreateTable(
                name: "UserTeam",
                columns: table => new
                {
                    TeamId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTeam", x => new { x.TeamId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserTeam_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTeam_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTeam_UserId",
                table: "UserTeam",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTeam");

            migrationBuilder.CreateTable(
                name: "TeamEntityUserEntity",
                columns: table => new
                {
                    MembersId = table.Column<long>(type: "bigint", nullable: false),
                    TeamsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamEntityUserEntity", x => new { x.MembersId, x.TeamsId });
                    table.ForeignKey(
                        name: "FK_TeamEntityUserEntity_Teams_TeamsId",
                        column: x => x.TeamsId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamEntityUserEntity_Users_MembersId",
                        column: x => x.MembersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamEntityUserEntity_TeamsId",
                table: "TeamEntityUserEntity",
                column: "TeamsId");
        }
    }
}
