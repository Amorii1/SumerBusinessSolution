using Microsoft.EntityFrameworkCore.Migrations;

namespace SumerBusinessSolution.Migrations
{
    public partial class updatingInvTransferTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovedById",
                table: "InvTransfer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "InvTransfer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvTransfer_ApprovedById",
                table: "InvTransfer",
                column: "ApprovedById");

            migrationBuilder.AddForeignKey(
                name: "FK_InvTransfer_AspNetUsers_ApprovedById",
                table: "InvTransfer",
                column: "ApprovedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvTransfer_AspNetUsers_ApprovedById",
                table: "InvTransfer");

            migrationBuilder.DropIndex(
                name: "IX_InvTransfer_ApprovedById",
                table: "InvTransfer");

            migrationBuilder.DropColumn(
                name: "ApprovedById",
                table: "InvTransfer");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "InvTransfer");
        }
    }
}
