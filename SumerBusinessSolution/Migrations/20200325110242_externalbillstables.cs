using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SumerBusinessSolution.Migrations
{
    public partial class externalbillstables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalBillHeader",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustId = table.Column<int>(nullable: true),
                    Status = table.Column<string>(nullable: false),
                    TotalAmt = table.Column<double>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    TotalNetAmt = table.Column<double>(nullable: false),
                    PaidAmt = table.Column<double>(nullable: false),
                    CreatedDataTime = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalBillHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalBillHeader_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExternalBillHeader_Customer_CustId",
                        column: x => x.CustId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExternalBillItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<int>(nullable: false),
                    ProdName = table.Column<string>(nullable: true),
                    Qty = table.Column<double>(nullable: false),
                    UnitPrice = table.Column<double>(nullable: false),
                    TotalAmt = table.Column<double>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalBillItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalBillItems_ExternalBillHeader_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "ExternalBillHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalBillHeader_CreatedById",
                table: "ExternalBillHeader",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalBillHeader_CustId",
                table: "ExternalBillHeader",
                column: "CustId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalBillItems_HeaderId",
                table: "ExternalBillItems",
                column: "HeaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalBillItems");

            migrationBuilder.DropTable(
                name: "ExternalBillHeader");
        }
    }
}
