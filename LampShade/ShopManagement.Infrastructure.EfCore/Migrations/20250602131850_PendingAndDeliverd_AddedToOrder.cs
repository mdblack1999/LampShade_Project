using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopManagement.Infrastructure.EfCore.Migrations
{
    public partial class PendingAndDeliverd_AddedToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Delivered",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
            migrationBuilder.AddColumn<bool>(
                name: "Pending",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Delivered",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Pending",
                table: "Orders");
        }
    }
}
