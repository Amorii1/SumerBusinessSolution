using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SumerBusinessSolution.Migrations
{
    public partial class creatingBillPaymentTable05 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             
 

            
            migrationBuilder.CreateTable(
                name: "BillPayment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillHeaderId = table.Column<int>(nullable: false),
                    CustId = table.Column<int>(nullable: true),
                    PaidAmt = table.Column<double>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillPayment_BillHeader_BillHeaderId",
                        column: x => x.BillHeaderId,
                        principalTable: "BillHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillPayment_Customer_CustId",
                        column: x => x.CustId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

           
              migrationBuilder.CreateIndex(
                name: "IX_BillPayment_BillHeaderId",
                table: "BillPayment",
                column: "BillHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_BillPayment_CustId",
                table: "BillPayment",
                column: "CustId");
 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BillItems");

            migrationBuilder.DropTable(
                name: "BillPayment");

            migrationBuilder.DropTable(
                name: "CustAcc");

            migrationBuilder.DropTable(
                name: "IncomingGood");

            migrationBuilder.DropTable(
                name: "InvStockQty");

            migrationBuilder.DropTable(
                name: "InvTransaction");

            migrationBuilder.DropTable(
                name: "InvTransfer");

            migrationBuilder.DropTable(
                name: "ProdImg");

            migrationBuilder.DropTable(
                name: "TempProdImg");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "BillHeader");

            migrationBuilder.DropTable(
                name: "InvTransferHeader");

            migrationBuilder.DropTable(
                name: "ProdInfo");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Warehouse");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "WhType");
        }
    }
}
