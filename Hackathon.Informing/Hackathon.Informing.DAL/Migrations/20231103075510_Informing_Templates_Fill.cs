using Hackathon.DAL.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.Informing.DAL.Migrations
{
    public partial class Informing_Templates_Fill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlFile("Migrations/Scripts/20231103075510_Informing_Templates_Fill.sql");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("truncate public.\"InformingTemplates\" cascade;");
        }
    }
}
