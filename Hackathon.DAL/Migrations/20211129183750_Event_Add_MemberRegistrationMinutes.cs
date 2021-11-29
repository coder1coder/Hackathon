using Microsoft.EntityFrameworkCore.Migrations;

namespace Hackathon.DAL.Migrations
{
    public partial class Event_Add_MemberRegistrationMinutes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemberRegistrationMinutes",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberRegistrationMinutes",
                table: "Events");
        }
    }
}
