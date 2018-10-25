using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoSite.Migrations.Migrations
{
    public partial class NoLimits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxLimit",
                table: "PropertyItems");

            migrationBuilder.DropColumn(
                name: "MinLimit",
                table: "PropertyItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxLimit",
                table: "PropertyItems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinLimit",
                table: "PropertyItems",
                nullable: true);
        }
    }
}
