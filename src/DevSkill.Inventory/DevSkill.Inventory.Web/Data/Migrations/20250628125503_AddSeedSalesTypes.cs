using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedSalesTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SaleTypes",
                columns: new[] { "Id", "PriceName" },
                values: new object[,]
                {
                    { new Guid("485a4002-097f-4864-a193-9017dc5230c5"), "MRP" },
                    { new Guid("ff7cc7d7-2694-4512-a425-3e9cce91caa5"), "Wholesale" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SaleTypes",
                keyColumn: "Id",
                keyValue: new Guid("485a4002-097f-4864-a193-9017dc5230c5"));

            migrationBuilder.DeleteData(
                table: "SaleTypes",
                keyColumn: "Id",
                keyValue: new Guid("ff7cc7d7-2694-4512-a425-3e9cce91caa5"));
        }
    }
}
