using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoSite.Migrations.Migrations
{
    public partial class UniqueNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SiteContent",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_SiteContent_Name",
                table: "SiteContent",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyItems_Name_ClassItemId",
                table: "PropertyItems",
                columns: new[] { "Name", "ClassItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassItems_Name_SiteContentId",
                table: "ClassItems",
                columns: new[] { "Name", "SiteContentId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SiteContent_Name",
                table: "SiteContent");

            migrationBuilder.DropIndex(
                name: "IX_PropertyItems_Name_ClassItemId",
                table: "PropertyItems");

            migrationBuilder.DropIndex(
                name: "IX_ClassItems_Name_SiteContentId",
                table: "ClassItems");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SiteContent",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
