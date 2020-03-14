using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SumerBusinessSolution.Migrations
{
    public partial class createCustomerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustAcc",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustId = table.Column<int>(nullable: false),
                    Paid = table.Column<double>(nullable: false),
                    Debt = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustAcc", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(nullable: true),
                    ContactName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    PhoneNo = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustAcc");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
