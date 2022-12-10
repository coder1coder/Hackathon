using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.DAL.Migrations
{
    public partial class FriendshipEntity_Add_Id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("create extension if not exists \"uuid-ossp\"");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Friendships",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Friendships",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Friendships");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Friendships",
                type: "integer",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");
        }
    }
}
