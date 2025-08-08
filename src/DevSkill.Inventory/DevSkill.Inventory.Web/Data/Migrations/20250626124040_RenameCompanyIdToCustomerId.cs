using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    public partial class RenameCompanyIdToCustomerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop old FK
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Companies_CompanyId",
                table: "Products");

            // Rename
            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Products",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CompanyId",
                table: "Products",
                newName: "IX_Products_CustomerId");

            // Make CustomerId nullable temporarily
            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            // Create Customers table
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            // Copy data from Companies (optional)
            migrationBuilder.Sql(@"
        INSERT INTO Customers (Id, Name, CompanyName, MobileNumber, Address, Email, OpeningBalance, CreatedDate)
        SELECT Id, Name, Name, '', '', '', 0, GETDATE()
        FROM Companies
    ");

            // Drop Companies
            migrationBuilder.DropTable(name: "Companies");

            // Add new FK
            migrationBuilder.AddForeignKey(
                name: "FK_Products_Customers_CustomerId",
                table: "Products",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. Drop foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Customers_CustomerId",
                table: "Products");

            // 2. Drop Customers table
            migrationBuilder.DropTable(
                name: "Customers");

            // 3. Rename the column and index back to CompanyId
            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Products",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CustomerId",
                table: "Products",
                newName: "IX_Products_CompanyId");

            // 4. Recreate Companies table (structure only)
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            // 5. Add foreign key to Companies
            migrationBuilder.AddForeignKey(
                name: "FK_Products_Companies_CompanyId",
                table: "Products",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
