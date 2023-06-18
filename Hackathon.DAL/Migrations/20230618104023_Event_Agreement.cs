using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.DAL.Migrations
{
    public partial class Event_Agreement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DevelopmentMinutes",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MemberRegistrationMinutes",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Rules",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TeamPresentationMinutes",
                table: "Events");

            migrationBuilder.CreateTable(
                name: "EventAgreements",
                columns: table => new
                {
                    EventId = table.Column<long>(type: "bigint", nullable: false),
                    Rules = table.Column<string>(type: "text", nullable: true),
                    RequiresConfirmation = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventAgreements", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_EventAgreements_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventAgreementUsers",
                columns: table => new
                {
                    AgreementId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventAgreementUsers", x => new { x.AgreementId, x.UserId });
                    table.ForeignKey(
                        name: "FK_EventAgreementUsers_EventAgreements_AgreementId",
                        column: x => x.AgreementId,
                        principalTable: "EventAgreements",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventAgreementUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventAgreementUsers_UserId",
                table: "EventAgreementUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventAgreementUsers");

            migrationBuilder.DropTable(
                name: "EventAgreements");

            migrationBuilder.AddColumn<int>(
                name: "DevelopmentMinutes",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MemberRegistrationMinutes",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Rules",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamPresentationMinutes",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
