using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SumerBusinessSolution.Migrations
{
    public partial class correctingSpelling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CraetedDataTime",
                table: "BillHeader");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDataTime",
                table: "BillHeader",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDataTime",
                table: "BillHeader");

            migrationBuilder.AddColumn<DateTime>(
                name: "CraetedDataTime",
                table: "BillHeader",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
