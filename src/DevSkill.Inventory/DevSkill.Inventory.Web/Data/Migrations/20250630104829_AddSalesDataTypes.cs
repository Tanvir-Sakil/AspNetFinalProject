using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSalesDataTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropColumn(
                name: "SalesType",
                table: "Sales");

            
            migrationBuilder.AddColumn<Guid>(
                name: "SalesType",
                table: "Sales",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);

            
            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "Sales");

           
            migrationBuilder.AddColumn<Guid>(
                name: "CustomerID",
                table: "Sales",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropColumn(
                name: "SalesType",
                table: "Sales");

            migrationBuilder.AddColumn<int>(
                name: "SalesType",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);

           
            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "Sales");

           
            migrationBuilder.AddColumn<int>(
                name: "CustomerID",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
