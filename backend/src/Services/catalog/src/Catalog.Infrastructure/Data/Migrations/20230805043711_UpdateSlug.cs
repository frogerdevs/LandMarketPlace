using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSlug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "ProductDiscount",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ProductDiscount",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDiscount_Slug",
                table: "ProductDiscount",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Slug",
                table: "Product",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Slug",
                table: "Category",
                column: "Slug");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductDiscount_Slug",
                table: "ProductDiscount");

            migrationBuilder.DropIndex(
                name: "IX_Product_Slug",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Category_Slug",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "ProductDiscount");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProductDiscount");
        }
    }
}
