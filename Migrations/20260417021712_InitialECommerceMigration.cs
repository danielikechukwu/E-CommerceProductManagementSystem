using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_CommerceProductManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialECommerceMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    price = table.Column<decimal>(type: "numeric(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    full_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    price = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_products_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                    table.ForeignKey(
                        name: "fk_orders_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_items_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_items_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "description", "name", "price" },
                values: new object[,]
                {
                    { 1, "Electronic devices and gadgets", "Electronics", 0m },
                    { 2, "Various genres of books and literature", "Books", 0m },
                    { 3, "Men's and women's apparel", "Clothing", 0m }
                });

            migrationBuilder.InsertData(
                table: "customers",
                columns: new[] { "id", "email", "full_name" },
                values: new object[,]
                {
                    { 1, "john@example.com", "John Doe" },
                    { 2, "jane@example.com", "Jane Smith" }
                });

            migrationBuilder.InsertData(
                table: "orders",
                columns: new[] { "id", "amount", "customer_id", "date" },
                values: new object[,]
                {
                    { 1, 1540.00m, 1, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 2, 840.00m, 2, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "category_id", "description", "name", "price" },
                values: new object[,]
                {
                    { 1, 1, "High performance laptop", "Laptop", 1500m },
                    { 2, 1, "Latest model smartphone", "Smartphone", 800m },
                    { 3, 2, "Comprehensive guide to ASP.NET Core", "ASP.NET Core Book", 40m }
                });

            migrationBuilder.InsertData(
                table: "order_items",
                columns: new[] { "id", "order_id", "product_id", "quantity", "unit_price" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, 1500.00m },
                    { 2, 1, 3, 1, 40.00m },
                    { 3, 2, 2, 1, 800.00m },
                    { 4, 2, 3, 1, 40.00m }
                });

            migrationBuilder.CreateIndex(
                name: "ix_order_items_order_id",
                table: "order_items",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_items_product_id",
                table: "order_items",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_customer_id",
                table: "orders",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_category_id",
                table: "products",
                column: "category_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_items");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
