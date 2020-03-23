using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SumerBusinessSolution.Migrations
{
    public partial class justtesting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestIncomingGood",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WhId = table.Column<int>(nullable: true),
                    ProdId = table.Column<int>(nullable: true),
                    Qty = table.Column<double>(nullable: false),
                    UOM = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestIncomingGood", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestIncomingGood_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestIncomingGood_ProdInfo_ProdId",
                        column: x => x.ProdId,
                        principalTable: "ProdInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestIncomingGood_Warehouse_WhId",
                        column: x => x.WhId,
                        principalTable: "Warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestIncomingGood_CreatedById",
                table: "TestIncomingGood",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TestIncomingGood_ProdId",
                table: "TestIncomingGood",
                column: "ProdId");

            migrationBuilder.CreateIndex(
                name: "IX_TestIncomingGood_WhId",
                table: "TestIncomingGood",
                column: "WhId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestIncomingGood");
        }
    }
}
