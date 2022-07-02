using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevInSales.Core.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, nullable: false),
                    SuggestedPrice = table.Column<decimal>(type: "decimal(18,2)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuyerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SellerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaleDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Initials = table.Column<string>(type: "varchar(2)", unicode: false, maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SaleProducts_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    Cep = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Complement = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: false),
                    DeliveryForecast = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deliveries_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Deliveries_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "SuggestedPrice" },
                values: new object[,]
                {
                    { 1, "Coca-Cola", 3.5m },
                    { 2, "cerveja Bohemia", 3.99m },
                    { 3, "Cerveja Itaipava", 3.59m },
                    { 4, "Ceveja Spaten", 3.49m },
                    { 5, "Cerveja Heineken", 5.59m },
                    { 6, "Cerveja Corona", 5.99m },
                    { 7, "Cerveja Stella", 3.19m },
                    { 8, "Cerveja Amstel", 3.49m },
                    { 9, "Cerveja Budweiser", 4.19m },
                    { 10, "Cerveja Brahma", 3.79m }
                });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Initials", "Name" },
                values: new object[,]
                {
                    { 1, "AC", "Acre" },
                    { 2, "AL", "Alagoas" },
                    { 3, "AP", "Amapá" },
                    { 4, "AM", "Amazonas" },
                    { 5, "BA", "Bahia" },
                    { 6, "CE", "Ceará" },
                    { 7, "DF", "Distrito Federal" },
                    { 8, "ES", "Espírito Santo" },
                    { 9, "GO", "Goiás" },
                    { 10, "MA", "Maranhão" },
                    { 11, "MT", "Mato Grosso" },
                    { 12, "MS", "Mato Grosso do Sul" },
                    { 13, "MG", "Minas Gerais" },
                    { 14, "PA", "Pará" },
                    { 15, "PB", "Paraíba" },
                    { 16, "PR", "Paraná" },
                    { 17, "PE", "Pernambuco" },
                    { 18, "PI", "Piauí" },
                    { 19, "RJ", "Rio de Janeiro" },
                    { 20, "RN", "Rio Grande do Norte" },
                    { 21, "RS", "Rio Grande do Sul" },
                    { 22, "RO", "Rondônia" },
                    { 23, "RR", "Roraima" },
                    { 24, "SC", "Santa Catarina" },
                    { 25, "SP", "São Paulo" },
                    { 26, "SE", "Sergipe" },
                    { 27, "TO", "Tocantins" }
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name", "StateId" },
                values: new object[,]
                {
                    { 1, "Florianópolis", 24 },
                    { 2, "Campinas", 25 },
                    { 3, "São Paulo", 25 },
                    { 4, "Rio de Janeiro", 19 }
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Cep", "CityId", "Complement", "Number", "Street" },
                values: new object[,]
                {
                    { 1, "13000000", 1, "Casa", 100, "Rua Aqui" },
                    { 2, "11111222", 2, "Ap", 200, "Rua Ali" },
                    { 3, "33333444", 3, "Chácara", 333, "Rua Rua" },
                    { 4, "55555666", 4, "Chácara", 333, "Rua 1" },
                    { 5, "77777888", 4, "Casa", 444, "Rua 2" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CityId",
                table: "Addresses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId",
                table: "Cities",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_AddressId",
                table: "Deliveries",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_SaleId",
                table: "Deliveries",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleProducts_ProductId",
                table: "SaleProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleProducts_SaleId",
                table: "SaleProducts",
                column: "SaleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropTable(
                name: "SaleProducts");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "States");
        }
    }
}
