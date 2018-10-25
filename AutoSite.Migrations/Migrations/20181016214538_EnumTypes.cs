using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoSite.Migrations.Migrations
{
    public partial class EnumTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DataType",
                table: "PropertyItems",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DataType",
                table: "PropertyItems",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
