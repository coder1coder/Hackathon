using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hackathon.DAL.Migrations
{
    public partial class ApprovalApplications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ApprovalApplicationId",
                table: "Events",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApprovalApplications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationStatus = table.Column<byte>(type: "smallint", nullable: false),
                    SignerId = table.Column<long>(type: "bigint", nullable: false),
                    AuthorId = table.Column<long>(type: "bigint", nullable: false),
                    Comment = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    RequestedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DecisionAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalApplications_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_ApprovalApplicationId",
                table: "Events",
                column: "ApprovalApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalApplications_AuthorId",
                table: "ApprovalApplications",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_ApprovalApplications_ApprovalApplicationId",
                table: "Events",
                column: "ApprovalApplicationId",
                principalTable: "ApprovalApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_ApprovalApplications_ApprovalApplicationId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "ApprovalApplications");

            migrationBuilder.DropIndex(
                name: "IX_Events_ApprovalApplicationId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ApprovalApplicationId",
                table: "Events");
        }
    }
}
