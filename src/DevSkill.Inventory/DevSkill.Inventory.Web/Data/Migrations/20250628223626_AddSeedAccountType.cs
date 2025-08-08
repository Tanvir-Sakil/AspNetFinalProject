using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedAccountType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AccountTypes",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("2fedffd2-f8b5-41f8-bd12-e593ec86e89e"), true, "Cash" },
                    { new Guid("4123c6eb-3d4b-4e5e-8fe3-cc6a8859320e"), true, "Bank" },
                    { new Guid("4973b853-d6e2-42b3-acbf-8195d611b470"), true, "Mobile" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountTypes",
                keyColumn: "Id",
                keyValue: new Guid("2fedffd2-f8b5-41f8-bd12-e593ec86e89e"));

            migrationBuilder.DeleteData(
                table: "AccountTypes",
                keyColumn: "Id",
                keyValue: new Guid("4123c6eb-3d4b-4e5e-8fe3-cc6a8859320e"));

            migrationBuilder.DeleteData(
                table: "AccountTypes",
                keyColumn: "Id",
                keyValue: new Guid("4973b853-d6e2-42b3-acbf-8195d611b470"));
        }
    }
}
