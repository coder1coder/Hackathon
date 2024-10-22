using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.Logbook.DAL.Migrations
{
    public partial class Logbook_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE TABLE IF NOT EXISTS public.""EventLog"" (
            ""Id"" uuid NOT NULL,
            ""Type"" int4 NOT NULL,
            ""UserId"" int8 NULL,
            ""UserName"" text NULL,
            ""Description"" text NULL,
            ""Timestamp"" timestamptz NOT NULL,
                CONSTRAINT ""PK_EventLog"" PRIMARY KEY (""Id"")
                );

            CREATE INDEX IF NOT EXISTS ""IX_EventLog_Timestamp"" ON public.""EventLog"" USING btree (""Timestamp"");
            CREATE INDEX IF NOT EXISTS ""IX_EventLog_Type"" ON public.""EventLog"" USING btree (""Type"");
            CREATE INDEX IF NOT EXISTS ""IX_EventLog_UserId"" ON public.""EventLog"" USING btree (""UserId"");
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventLog");
        }
    }
}
