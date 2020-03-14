using Microsoft.EntityFrameworkCore.Migrations;

namespace SumerBusinessSolution.Migrations
{
    public partial class updatingCustomerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Customer");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Customer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactName",
                table: "Customer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Customer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Customer",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CreatedById",
                table: "Customer",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustAcc_CustId",
                table: "CustAcc",
                column: "CustId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustAcc_Customer_CustId",
                table: "CustAcc",
                column: "CustId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_AspNetUsers_CreatedById",
                table: "Customer",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustAcc_Customer_CustId",
                table: "CustAcc");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_AspNetUsers_CreatedById",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_CreatedById",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_CustAcc_CustId",
                table: "CustAcc");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Customer");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ContactName",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
