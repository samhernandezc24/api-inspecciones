using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Inspecciones.Migrations
{
    /// <inheritdoc />
    public partial class migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NoAplica",
                schema: "inspeccion",
                table: "InspeccionesCategoriasValues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                schema: "inspeccion",
                table: "InspeccionesCategoriasValues",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoAplica",
                schema: "inspeccion",
                table: "InspeccionesCategoriasValues");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                schema: "inspeccion",
                table: "InspeccionesCategoriasValues");
        }
    }
}
