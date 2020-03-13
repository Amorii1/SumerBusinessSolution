using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SumerBusinessSolution.Migrations
{
    public partial class creatingTransferHeaderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvTransfer_AspNetUsers_ApprovedById",
                table: "InvTransfer");

            migrationBuilder.DropForeignKey(
                name: "FK_InvTransfer_AspNetUsers_CreatedById",
                table: "InvTransfer");

            migrationBuilder.DropForeignKey(
                name: "FK_InvTransfer_Warehouse_FromWhId",
                table: "InvTransfer");

            migrationBuilder.DropForeignKey(
                name: "FK_InvTransfer_Warehouse_ToWhId",
                table: "InvTransfer");

            migrationBuilder.DropIndex(
                name: "IX_InvTransfer_ApprovedById",
                table: "InvTransfer");

            migrationBuilder.DropIndex(
                name: "IX_InvTransfer_CreatedById",
                table: "InvTransfer");

            migrationBuilder.DropIndex(
                name: "IX_InvTransfer_FromWhId",
                table: "InvTransfer");

            migrationBuilder.DropIndex(
                name: "IX_InvTransfer_ToWhId",
                table: "InvTransfer");

            migrationBuilder.DropColumn(
                name: "ApprovedById",
                table: "InvTransfer");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "InvTransfer");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "InvTransfer");

            migrationBuilder.DropColumn(
                name: "FromWhId",
                table: "InvTransfer");

            migrationBuilder.DropColumn(
                name: "ToWhId",
                table: "InvTransfer");

            migrationBuilder.DropColumn(
                name: "TransferStatus",
                table: "InvTransfer");

            migrationBuilder.AddColumn<int>(
                name: "HeaderId",
                table: "InvTransfer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InvTransferHeader",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromWhId = table.Column<int>(nullable: true),
                    ToWhId = table.Column<int>(nullable: true),
                    TransferStatus = table.Column<string>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: false),
                    ApprovedById = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvTransferHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvTransferHeader_AspNetUsers_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvTransferHeader_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvTransferHeader_Warehouse_FromWhId",
                        column: x => x.FromWhId,
                        principalTable: "Warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvTransferHeader_Warehouse_ToWhId",
                        column: x => x.ToWhId,
                        principalTable: "Warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvTransferHeader_ApprovedById",
                table: "InvTransferHeader",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_InvTransferHeader_CreatedById",
                table: "InvTransferHeader",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InvTransferHeader_FromWhId",
                table: "InvTransferHeader",
                column: "FromWhId");

            migrationBuilder.CreateIndex(
                name: "IX_InvTransferHeader_ToWhId",
                table: "InvTransferHeader",
                column: "ToWhId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvTransferHeader");

            migrationBuilder.DropColumn(
                name: "HeaderId",
                table: "InvTransfer");

            migrationBuilder.AddColumn<string>(
                name: "ApprovedById",
                table: "InvTransfer",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "InvTransfer",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "InvTransfer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FromWhId",
                table: "InvTransfer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToWhId",
                table: "InvTransfer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransferStatus",
                table: "InvTransfer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_InvTransfer_ApprovedById",
                table: "InvTransfer",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_InvTransfer_CreatedById",
                table: "InvTransfer",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InvTransfer_FromWhId",
                table: "InvTransfer",
                column: "FromWhId");

            migrationBuilder.CreateIndex(
                name: "IX_InvTransfer_ToWhId",
                table: "InvTransfer",
                column: "ToWhId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvTransfer_AspNetUsers_ApprovedById",
                table: "InvTransfer",
                column: "ApprovedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvTransfer_AspNetUsers_CreatedById",
                table: "InvTransfer",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvTransfer_Warehouse_FromWhId",
                table: "InvTransfer",
                column: "FromWhId",
                principalTable: "Warehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvTransfer_Warehouse_ToWhId",
                table: "InvTransfer",
                column: "ToWhId",
                principalTable: "Warehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
