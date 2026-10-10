using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comasy.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteStyles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteStyles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Element = table.Column<int>(type: "int", nullable: false),
                    FontFamily = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FontSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FontWeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextDecoration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BorderRadius = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteStyles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SiteStyles_Element",
                table: "SiteStyles",
                column: "Element",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteStyles");
        }
    }
}
