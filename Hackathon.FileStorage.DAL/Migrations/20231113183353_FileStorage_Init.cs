using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.FileStorage.DAL.Migrations
{
    public partial class FileStorage_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE TABLE if not exists public.""StorageFiles"" (
            ""Id"" uuid NOT NULL,
            ""BucketName"" text NOT NULL,
            ""FileName"" text NOT NULL,
            ""FilePath"" text NOT NULL,
            ""MimeType"" text NULL,
            ""Length"" int8 NULL,
            ""OwnerId"" int8 NULL,
            ""IsDeleted"" bool NOT NULL DEFAULT false,
            CONSTRAINT ""PK_StorageFiles"" PRIMARY KEY (""Id""));

            CREATE INDEX if not exists ""IX_StorageFiles_IsDeleted"" ON public.""StorageFiles"" USING btree (""IsDeleted"");
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StorageFiles");
        }
    }
}
