using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SumerBusinessSolution.Migrations
{
    public partial class createexternalbillpayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalBillPayment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalBillHeaderId = table.Column<int>(nullable: false),
                    CustId = table.Column<int>(nullable: true),
                    PaidAmt = table.Column<double>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalBillPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalBillPayment_Customer_CustId",
                        column: x => x.CustId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalBillPayment_ExternalBillHeader_ExternalBillHeaderId",
                        column: x => x.ExternalBillHeaderId,
                        principalTable: "ExternalBillHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalBillPayment_CustId",
                table: "ExternalBillPayment",
                column: "CustId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalBillPayment_ExternalBillHeaderId",
                table: "ExternalBillPayment",
                column: "ExternalBillHeaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalBillPayment");
        }
    }
}
