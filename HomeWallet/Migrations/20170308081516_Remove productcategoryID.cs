using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeWallet.Migrations
{
    public partial class RemoveproductcategoryID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCategoryID",
                schema: "dbo",
                table: "Categories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductCategoryID",
                schema: "dbo",
                table: "Categories",
                nullable: false,
                defaultValue: 0);
        }
    }
}
