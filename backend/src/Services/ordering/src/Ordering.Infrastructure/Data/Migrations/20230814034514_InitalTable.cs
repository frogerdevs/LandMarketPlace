using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitalTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddOn",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    DurationDays = table.Column<int>(type: "integer", nullable: false),
                    UpgradableFrom = table.Column<string>(type: "text", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddOn", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    DurationDays = table.Column<int>(type: "integer", nullable: false),
                    UpgradableFrom = table.Column<string>(type: "text", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DataType = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAddOn",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    AddOnId = table.Column<string>(type: "text", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    ActiveDurationDays = table.Column<int>(type: "integer", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric", nullable: false),
                    Tax = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    AddOnName = table.Column<string>(type: "text", nullable: true),
                    AddOnDescription = table.Column<string>(type: "text", nullable: true),
                    AddOnPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    AddOnDurationDays = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddOn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAddOn_AddOn_AddOnId",
                        column: x => x.AddOnId,
                        principalTable: "AddOn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSubscription",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    SubscriptionId = table.Column<string>(type: "text", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    ActiveDurationDays = table.Column<int>(type: "integer", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric", nullable: false),
                    Tax = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    SubscriptionName = table.Column<string>(type: "text", nullable: true),
                    SubscriptionDescription = table.Column<string>(type: "text", nullable: true),
                    SubscriptionPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    SubscriptionDurationDays = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSubscription_Subscription_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AddOnDetail",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    AddOnId = table.Column<string>(type: "text", nullable: true),
                    SubscriptionTypeId = table.Column<string>(type: "text", nullable: true),
                    AddOnValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddOnDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddOnDetail_AddOn_AddOnId",
                        column: x => x.AddOnId,
                        principalTable: "AddOn",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AddOnDetail_SubscriptionType_SubscriptionTypeId",
                        column: x => x.SubscriptionTypeId,
                        principalTable: "SubscriptionType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionDetail",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    SubscriptionId = table.Column<string>(type: "text", nullable: true),
                    SubscriptionTypeId = table.Column<string>(type: "text", nullable: true),
                    SubscriptionValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionDetail_SubscriptionType_SubscriptionTypeId",
                        column: x => x.SubscriptionTypeId,
                        principalTable: "SubscriptionType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubscriptionDetail_Subscription_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscription",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserAddOnHistory",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    UserAddonId = table.Column<string>(type: "text", nullable: true),
                    SubscriptionValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddOnHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAddOnHistory_UserAddOn_UserAddonId",
                        column: x => x.UserAddonId,
                        principalTable: "UserAddOn",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserSubscriptionHistory",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    UserSubscriptionId = table.Column<string>(type: "text", nullable: true),
                    SubscriptionValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscriptionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSubscriptionHistory_UserSubscription_UserSubscriptionId",
                        column: x => x.UserSubscriptionId,
                        principalTable: "UserSubscription",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddOnDetail_AddOnId",
                table: "AddOnDetail",
                column: "AddOnId");

            migrationBuilder.CreateIndex(
                name: "IX_AddOnDetail_SubscriptionTypeId",
                table: "AddOnDetail",
                column: "SubscriptionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionDetail_SubscriptionId",
                table: "SubscriptionDetail",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionDetail_SubscriptionTypeId",
                table: "SubscriptionDetail",
                column: "SubscriptionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddOn_AddOnId",
                table: "UserAddOn",
                column: "AddOnId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddOnHistory_UserAddonId",
                table: "UserAddOnHistory",
                column: "UserAddonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscription_SubscriptionId",
                table: "UserSubscription",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptionHistory_UserSubscriptionId",
                table: "UserSubscriptionHistory",
                column: "UserSubscriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddOnDetail");

            migrationBuilder.DropTable(
                name: "SubscriptionDetail");

            migrationBuilder.DropTable(
                name: "UserAddOnHistory");

            migrationBuilder.DropTable(
                name: "UserSubscriptionHistory");

            migrationBuilder.DropTable(
                name: "SubscriptionType");

            migrationBuilder.DropTable(
                name: "UserAddOn");

            migrationBuilder.DropTable(
                name: "UserSubscription");

            migrationBuilder.DropTable(
                name: "AddOn");

            migrationBuilder.DropTable(
                name: "Subscription");
        }
    }
}
