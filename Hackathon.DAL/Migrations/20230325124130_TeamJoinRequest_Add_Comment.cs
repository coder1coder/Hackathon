using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.DAL.Migrations
{
    public partial class TeamJoinRequest_Add_Comment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "TeamJoinRequests",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "TeamJoinRequests");
        }
    }
}
