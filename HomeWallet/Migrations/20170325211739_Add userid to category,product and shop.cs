using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeWallet.Migrations
{
    public partial class Adduseridtocategoryproductandshop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                schema: "dbo",
                table: "Shops",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                schema: "dbo",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                schema: "dbo",
                table: "Categories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shops_UserID",
                schema: "dbo",
                table: "Shops",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserID",
                schema: "dbo",
                table: "Products",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserID",
                schema: "dbo",
                table: "Categories",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_UserID",
                schema: "dbo",
                table: "Categories",
                column: "UserID",
                principalSchema: "dbo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_UserID",
                schema: "dbo",
                table: "Products",
                column: "UserID",
                principalSchema: "dbo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_AspNetUsers_UserID",
                schema: "dbo",
                table: "Shops",
                column: "UserID",
                principalSchema: "dbo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_UserID",
                schema: "dbo",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_UserID",
                schema: "dbo",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Shops_AspNetUsers_UserID",
                schema: "dbo",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Shops_UserID",
                schema: "dbo",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Products_UserID",
                schema: "dbo",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Categories_UserID",
                schema: "dbo",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UserID",
                schema: "dbo",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "UserID",
                schema: "dbo",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserID",
                schema: "dbo",
                table: "Categories");
        }
    }
}
