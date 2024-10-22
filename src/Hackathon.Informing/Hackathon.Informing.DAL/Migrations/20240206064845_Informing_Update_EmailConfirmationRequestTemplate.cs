using Hackathon.DAL.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.Informing.DAL.Migrations
{
    public partial class Informing_Update_EmailConfirmationRequestTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlFile("Migrations/Scripts/20240206064845_Informing_Update_EmailConfirmationRequestTemplate.sql");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlFile("Migrations/Scripts/20240206064845_Informing_Update_EmailConfirmationRequestTemplate__rollback.sql");
        }
    }
}
