using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Data.EF.Migrations
{
    public partial class RenameBillingAccountsBillingEnabledColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "billing_enabled",
                table: "billing_accounts",
                newName: "enabled");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "enabled",
                table: "billing_accounts",
                newName: "billing_enabled");
        }
    }
}
