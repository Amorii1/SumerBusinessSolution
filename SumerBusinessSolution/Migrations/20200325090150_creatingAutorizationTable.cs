using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SumerBusinessSolution.Migrations
{
    public partial class creatingAutorizationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestIncomingGood");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestIncomingGood",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProdId = table.Column<int>(type: "int", nullable: true),
                    Qty = table.Column<double>(type: "float", nullable: false),
                    UOM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhId = table.Column<int>(type: "int", nullable: true)
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
    }
}
