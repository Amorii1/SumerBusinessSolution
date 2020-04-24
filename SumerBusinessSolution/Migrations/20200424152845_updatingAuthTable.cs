using Microsoft.EntityFrameworkCore.Migrations;

namespace SumerBusinessSolution.Migrations
{
    public partial class updatingAuthTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateTrans",
                table: "RoleAuth");

            migrationBuilder.AddColumn<bool>(
                name: "CreateInGoods",
                table: "RoleAuth",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateInGoods",
                table: "RoleAuth");

            migrationBuilder.AddColumn<bool>(
                name: "CreateTrans",
                table: "RoleAuth",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
