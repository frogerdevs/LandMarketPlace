using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotDeals");

            migrationBuilder.AddColumn<string>(
                name: "Channel",
                table: "ProductDiscount",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Adsense",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Channel",
                table: "ProductDiscount");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Adsense");

            migrationBuilder.CreateTable(
                name: "HotDeals",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<string>(type: "text", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    Channel = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DealsFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DealsTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DiscountPercent = table.Column<int>(type: "integer", nullable: false),
                    DiscountPrice = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotDeals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotDeals_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotDeals_ProductId",
                table: "HotDeals",
                column: "ProductId");
        }
    }
}
