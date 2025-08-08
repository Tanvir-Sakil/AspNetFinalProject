using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSaleTypeRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SalesType",
                table: "Sales",
                newName: "SalesTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_SalesTypeId",
                table: "Sales",
                column: "SalesTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_SaleTypes_SalesTypeId",
                table: "Sales",
                column: "SalesTypeId",
                principalTable: "SaleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_SaleTypes_SalesTypeId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_SalesTypeId",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "SalesTypeId",
                table: "Sales",
                newName: "SalesType");
        }
    }
}
