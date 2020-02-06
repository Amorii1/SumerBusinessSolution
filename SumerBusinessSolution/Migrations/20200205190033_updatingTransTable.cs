using Microsoft.EntityFrameworkCore.Migrations;

namespace SumerBusinessSolution.Migrations
{
    public partial class updatingTransTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransferType",
                table: "InvTransaction");

            migrationBuilder.AddColumn<string>(
                name: "TransType",
                table: "InvTransaction",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransType",
                table: "InvTransaction");

            migrationBuilder.AddColumn<string>(
                name: "TransferType",
                table: "InvTransaction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
