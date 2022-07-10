using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.DAL.Migrations
{
    public partial class Event_Description_Award_Required_SetNullAsEmpty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE public.\"Events\" SET \"Description\" = '' WHERE \"Description\" IS NULL;");
            migrationBuilder.Sql("UPDATE public.\"Events\" SET \"Award\" = '' WHERE \"Award\" IS NULL;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
