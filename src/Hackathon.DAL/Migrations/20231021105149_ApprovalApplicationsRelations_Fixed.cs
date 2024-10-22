using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.DAL.Migrations
{
    public partial class ApprovalApplicationsRelations_Fixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_ApprovalApplications_ApprovalApplicationId",
                table: "Events");

            migrationBuilder.AlterColumn<long>(
                name: "SignerId",
                table: "ApprovalApplications",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalApplications_SignerId",
                table: "ApprovalApplications",
                column: "SignerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalApplications_Users_SignerId",
                table: "ApprovalApplications",
                column: "SignerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_ApprovalApplications_ApprovalApplicationId",
                table: "Events",
                column: "ApprovalApplicationId",
                principalTable: "ApprovalApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalApplications_Users_SignerId",
                table: "ApprovalApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_ApprovalApplications_ApprovalApplicationId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalApplications_SignerId",
                table: "ApprovalApplications");

            migrationBuilder.AlterColumn<long>(
                name: "SignerId",
                table: "ApprovalApplications",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_ApprovalApplications_ApprovalApplicationId",
                table: "Events",
                column: "ApprovalApplicationId",
                principalTable: "ApprovalApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
