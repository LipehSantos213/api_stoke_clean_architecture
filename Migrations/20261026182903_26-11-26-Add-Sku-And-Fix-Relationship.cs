using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_stoke.Migrations
{
    /// <inheritdoc />
    public partial class _261126AddSkuAndFixRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movements_products_product_id",
                table: "movements");

            migrationBuilder.DropIndex(
                name: "IX_registration_clients_panel_id",
                table: "registration_clients");

            migrationBuilder.DropIndex(
                name: "IX_products_sku",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_movements_product_id",
                table: "movements");

            migrationBuilder.DropColumn(
                name: "sku",
                table: "products");

            migrationBuilder.DropColumn(
                name: "product_id",
                table: "movements");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "cart_items",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PriceTotal",
                table: "cart_items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "cart_items",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "sku_products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sku = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    status = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sku_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_sku_products_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sku_cart_item",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cart_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sku_id = table.Column<Guid>(type: "uuid", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sku_cart_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_sku_cart_item_cart_items_cart_item_id",
                        column: x => x.cart_item_id,
                        principalTable: "cart_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sku_cart_item_sku_products_sku_id",
                        column: x => x.sku_id,
                        principalTable: "sku_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sku_movement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sku_product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    movement_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sku_movement", x => x.id);
                    table.ForeignKey(
                        name: "FK_sku_movement_movements_movement_id",
                        column: x => x.movement_id,
                        principalTable: "movements",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sku_movement_sku_products_sku_product_id",
                        column: x => x.sku_product_id,
                        principalTable: "sku_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_registration_clients_panel_id_cart_id",
                table: "registration_clients",
                columns: new[] { "panel_id", "cart_id" });

            migrationBuilder.CreateIndex(
                name: "IX_sku_cart_item_cart_item_id_sku_id",
                table: "sku_cart_item",
                columns: new[] { "cart_item_id", "sku_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sku_cart_item_sku_id",
                table: "sku_cart_item",
                column: "sku_id");

            migrationBuilder.CreateIndex(
                name: "IX_sku_movement_movement_id",
                table: "sku_movement",
                column: "movement_id");

            migrationBuilder.CreateIndex(
                name: "IX_sku_movement_sku_product_id_movement_id",
                table: "sku_movement",
                columns: new[] { "sku_product_id", "movement_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sku_products_product_id",
                table: "sku_products",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_sku_products_sku",
                table: "sku_products",
                column: "sku",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sku_cart_item");

            migrationBuilder.DropTable(
                name: "sku_movement");

            migrationBuilder.DropTable(
                name: "sku_products");

            migrationBuilder.DropIndex(
                name: "IX_registration_clients_panel_id_cart_id",
                table: "registration_clients");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "cart_items");

            migrationBuilder.DropColumn(
                name: "PriceTotal",
                table: "cart_items");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "cart_items");

            migrationBuilder.AddColumn<string>(
                name: "sku",
                table: "products",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "product_id",
                table: "movements",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_registration_clients_panel_id",
                table: "registration_clients",
                column: "panel_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_sku",
                table: "products",
                column: "sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_movements_product_id",
                table: "movements",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_movements_products_product_id",
                table: "movements",
                column: "product_id",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
