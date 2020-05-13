using Microsoft.EntityFrameworkCore.Migrations;

namespace Brasserie.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brasseries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brasseries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grossiste",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grossiste", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Biere",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(nullable: true),
                    DegreAlcool = table.Column<double>(nullable: false),
                    Prix = table.Column<double>(nullable: false),
                    BrasserieId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biere", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Biere_Brasseries_BrasserieId",
                        column: x => x.BrasserieId,
                        principalTable: "Brasseries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockGrossiste",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrossisteId = table.Column<int>(nullable: false),
                    BiereId = table.Column<int>(nullable: false),
                    Quantite = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockGrossiste", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockGrossiste_Biere_BiereId",
                        column: x => x.BiereId,
                        principalTable: "Biere",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockGrossiste_Grossiste_GrossisteId",
                        column: x => x.GrossisteId,
                        principalTable: "Grossiste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Brasseries",
                columns: new[] { "Id", "Nom" },
                values: new object[,]
                {
                    { 1, "Abbaye de Leffe" },
                    { 2, "Abbaye de Chimay" }
                });

            migrationBuilder.InsertData(
                table: "Grossiste",
                columns: new[] { "Id", "Nom" },
                values: new object[,]
                {
                    { 1, "GeneDrinks" },
                    { 2, "Onidrinks" }
                });

            migrationBuilder.InsertData(
                table: "Biere",
                columns: new[] { "Id", "BrasserieId", "DegreAlcool", "Nom", "Prix" },
                values: new object[,]
                {
                    { 1, 1, 6.5999999999999996, "Leffe Blonde", 2.2000000000000002 },
                    { 2, 1, 7.5999999999999996, "Leffe Brune", 2.7999999999999998 },
                    { 3, 2, 6.5999999999999996, "Chimay Bleue", 3.3999999999999999 },
                    { 4, 2, 6.5999999999999996, "Chimay Blonde", 3.2000000000000002 },
                    { 5, 2, 8.0, "Chimay Rouge", 3.0 }
                });

            migrationBuilder.InsertData(
                table: "StockGrossiste",
                columns: new[] { "Id", "BiereId", "GrossisteId", "Quantite" },
                values: new object[,]
                {
                    { 1, 1, 1, 10 },
                    { 6, 1, 2, 10 },
                    { 2, 2, 1, 15 },
                    { 3, 3, 1, 10 },
                    { 7, 3, 2, 10 },
                    { 4, 4, 1, 5 },
                    { 8, 4, 1, 10 },
                    { 5, 5, 1, 30 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Biere_BrasserieId",
                table: "Biere",
                column: "BrasserieId");

            migrationBuilder.CreateIndex(
                name: "IX_StockGrossiste_BiereId",
                table: "StockGrossiste",
                column: "BiereId");

            migrationBuilder.CreateIndex(
                name: "IX_StockGrossiste_GrossisteId",
                table: "StockGrossiste",
                column: "GrossisteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockGrossiste");

            migrationBuilder.DropTable(
                name: "Biere");

            migrationBuilder.DropTable(
                name: "Grossiste");

            migrationBuilder.DropTable(
                name: "Brasseries");
        }
    }
}
