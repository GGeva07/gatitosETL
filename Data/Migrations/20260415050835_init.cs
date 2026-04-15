using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gatitosEtl.Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DimCiudades",
                columns: table => new
                {
                    id_ciudad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimCiudades", x => x.id_ciudad);
                });

            migrationBuilder.CreateTable(
                name: "DimFechas",
                columns: table => new
                {
                    id_fecha = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dia = table.Column<int>(type: "int", nullable: false),
                    mes = table.Column<int>(type: "int", nullable: false),
                    anio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimFechas", x => x.id_fecha);
                });

            migrationBuilder.CreateTable(
                name: "DimGatos",
                columns: table => new
                {
                    id_gato = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    raza = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimGatos", x => x.id_gato);
                });

            migrationBuilder.CreateTable(
                name: "DimPersonas",
                columns: table => new
                {
                    id_persona = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    idCiudad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimPersonas", x => x.id_persona);
                    table.ForeignKey(
                        name: "FK_DimPersonas_DimCiudades_idCiudad",
                        column: x => x.idCiudad,
                        principalTable: "DimCiudades",
                        principalColumn: "id_ciudad",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DimCiudades_nombre",
                table: "DimCiudades",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DimFechas_dia_mes_anio",
                table: "DimFechas",
                columns: new[] { "dia", "mes", "anio" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DimPersonas_idCiudad",
                table: "DimPersonas",
                column: "idCiudad");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DimFechas");

            migrationBuilder.DropTable(
                name: "DimGatos");

            migrationBuilder.DropTable(
                name: "DimPersonas");

            migrationBuilder.DropTable(
                name: "DimCiudades");
        }
    }
}
