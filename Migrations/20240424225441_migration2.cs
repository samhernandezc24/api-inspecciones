using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Inspecciones.Migrations
{
    /// <inheritdoc />
    public partial class migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdUnidadPlacaTipo",
                schema: "inspeccion",
                table: "Unidades",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Placa",
                schema: "inspeccion",
                table: "Unidades",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnidadPlacaTipoName",
                schema: "inspeccion",
                table: "Unidades",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdUnidadPlacaTipo",
                schema: "inspeccion",
                table: "Unidades");

            migrationBuilder.DropColumn(
                name: "Placa",
                schema: "inspeccion",
                table: "Unidades");

            migrationBuilder.DropColumn(
                name: "UnidadPlacaTipoName",
                schema: "inspeccion",
                table: "Unidades");
        }
    }
}
