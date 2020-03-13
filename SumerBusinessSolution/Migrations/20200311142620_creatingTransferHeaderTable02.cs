using Microsoft.EntityFrameworkCore.Migrations;

namespace SumerBusinessSolution.Migrations
{
    public partial class creatingTransferHeaderTable02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_InvTransfer_HeaderId",
                table: "InvTransfer",
                column: "HeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvTransfer_InvTransferHeader_HeaderId",
                table: "InvTransfer",
                column: "HeaderId",
                principalTable: "InvTransferHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvTransfer_InvTransferHeader_HeaderId",
                table: "InvTransfer");

            migrationBuilder.DropIndex(
                name: "IX_InvTransfer_HeaderId",
                table: "InvTransfer");
        }
    }
}
