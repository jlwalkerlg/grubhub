using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Data.EF.Migrations
{
    public partial class RemoveIsDeleteColumnFromMenuCategoriesAndItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "menu_items");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "menu_categories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "menu_items",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "menu_categories",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
