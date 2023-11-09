using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;

#nullable disable
namespace Hackathon.Informing.DAL.Migrations
{
    public partial class Informing_Init : Migration
    {
        //TODO: не забудь меня здесь раскомментировать
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
             * изначально таблица Notifications создавалась в рамках основного контекста приложения
             * поэтому при попытке создать таблицу которая уже существует мы получим ошибку
             *
             * скрипт оставлен для тех случаев, когда миграции применяются к новой базе данных
             *
             */
            
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    OwnerId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    Data = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");
        }
    }
}
