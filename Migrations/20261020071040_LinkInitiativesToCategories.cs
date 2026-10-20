using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YouthHub.Migrations
{
    /// <inheritdoc />
    public partial class LinkInitiativesToCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Initiatives",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Initiatives_CategoryId",
                table: "Initiatives",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Initiatives_Categories_CategoryId",
                table: "Initiatives",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Initiatives_Categories_CategoryId",
                table: "Initiatives");

            migrationBuilder.DropIndex(
                name: "IX_Initiatives_CategoryId",
                table: "Initiatives");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Initiatives");
        }
    }
}
