using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeWallet.Migrations
{
    public partial class DeleteproductunnecessaryID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCategoryID",
                schema: "dbo",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductCategoryID",
                schema: "dbo",
                table: "Products",
                nullable: false,
                defaultValue: 0);
        }
    }
}
