using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddChangeStaffEntityDepartmentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
        IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Staffs_Departments_DepartmentId1')
            ALTER TABLE Staffs DROP CONSTRAINT FK_Staffs_Departments_DepartmentId1;
    ");
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Staffs_DepartmentId1')
            DROP INDEX IX_Staffs_DepartmentId1 ON Staffs;
    ");
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'DepartmentId1' AND Object_ID = Object_ID(N'Staffs'))
            ALTER TABLE Staffs DROP COLUMN DepartmentId1;
    ");

            // Drop old DepartmentId index (safely)
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Staffs_DepartmentId')
            DROP INDEX IX_Staffs_DepartmentId ON Staffs;
    ");

            // Drop old DepartmentId column
            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Staffs");

            // Add the new Guid column
            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "Staffs",
                type: "uniqueidentifier",
                nullable: true);

            // Recreate index and FK
            migrationBuilder.CreateIndex(
                name: "IX_Staffs_DepartmentId",
                table: "Staffs",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Departments_DepartmentId",
                table: "Staffs",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
        IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Staffs_Departments_DepartmentId')
            ALTER TABLE Staffs DROP CONSTRAINT FK_Staffs_Departments_DepartmentId;
    ");
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Staffs_DepartmentId')
            DROP INDEX IX_Staffs_DepartmentId ON Staffs;
    ");

            // Drop the new Guid column
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'DepartmentId' AND Object_ID = Object_ID(N'Staffs'))
            ALTER TABLE Staffs DROP COLUMN DepartmentId;
    ");

            // Add back the old int column
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Staffs",
                type: "int",
                nullable: true);

            // Recreate index and FK for int column
            migrationBuilder.CreateIndex(
                name: "IX_Staffs_DepartmentId",
                table: "Staffs",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Departments_DepartmentId",
                table: "Staffs",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
