using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Data.EF.Migrations
{
    public partial class RemoveOrderItemMenuItemRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_items_menu_items_menu_item_id",
                table: "order_items");

            migrationBuilder.DropIndex(
                name: "IX_order_items_menu_item_id",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "menu_item_id",
                table: "order_items");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "menu_item_id",
                table: "order_items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_order_items_menu_item_id",
                table: "order_items",
                column: "menu_item_id");

            migrationBuilder.AddForeignKey(
                name: "FK_order_items_menu_items_menu_item_id",
                table: "order_items",
                column: "menu_item_id",
                principalTable: "menu_items",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
