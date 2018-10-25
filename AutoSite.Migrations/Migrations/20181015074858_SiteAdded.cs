using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoSite.Migrations.Migrations
{
    public partial class SiteAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyItems_ClassItems_ClassItemId",
                table: "PropertyItems");

            migrationBuilder.AlterColumn<int>(
                name: "ClassItemId",
                table: "PropertyItems",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SiteContentId",
                table: "ClassItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SiteContent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteContent", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassItems_SiteContentId",
                table: "ClassItems",
                column: "SiteContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassItems_SiteContent_SiteContentId",
                table: "ClassItems",
                column: "SiteContentId",
                principalTable: "SiteContent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyItems_ClassItems_ClassItemId",
                table: "PropertyItems",
                column: "ClassItemId",
                principalTable: "ClassItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassItems_SiteContent_SiteContentId",
                table: "ClassItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyItems_ClassItems_ClassItemId",
                table: "PropertyItems");

            migrationBuilder.DropTable(
                name: "SiteContent");

            migrationBuilder.DropIndex(
                name: "IX_ClassItems_SiteContentId",
                table: "ClassItems");

            migrationBuilder.DropColumn(
                name: "SiteContentId",
                table: "ClassItems");

            migrationBuilder.AlterColumn<int>(
                name: "ClassItemId",
                table: "PropertyItems",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyItems_ClassItems_ClassItemId",
                table: "PropertyItems",
                column: "ClassItemId",
                principalTable: "ClassItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
