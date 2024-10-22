using Hackathon.Common.Models.Event;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.DAL.Migrations
{
    public partial class Event_Add_Tasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<EventTaskItem[]>(
                name: "Tasks",
                table: "Events",
                type: "jsonb",
                nullable: false,
                defaultValue: new EventTaskItem[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tasks",
                table: "Events");
        }
    }
}
