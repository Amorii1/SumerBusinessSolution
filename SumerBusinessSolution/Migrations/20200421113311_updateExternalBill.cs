using Microsoft.EntityFrameworkCore.Migrations;

namespace SumerBusinessSolution.Migrations
{
    public partial class updateExternalBill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExternal",
                table: "ExternalBillItems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProdId",
                table: "ExternalBillItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalBillItems_ProdId",
                table: "ExternalBillItems",
                column: "ProdId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalBillItems_ProdInfo_ProdId",
                table: "ExternalBillItems",
                column: "ProdId",
                principalTable: "ProdInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalBillItems_ProdInfo_ProdId",
                table: "ExternalBillItems");

            migrationBuilder.DropIndex(
                name: "IX_ExternalBillItems_ProdId",
                table: "ExternalBillItems");

            migrationBuilder.DropColumn(
                name: "IsExternal",
                table: "ExternalBillItems");

            migrationBuilder.DropColumn(
                name: "ProdId",
                table: "ExternalBillItems");
        }
    }
}
