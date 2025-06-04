using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopManagement.Infrastructure.EfCore.Migrations
{
    public partial class Checked_Order_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Checked",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Checked",
                table: "Orders");
        }
    }
}
