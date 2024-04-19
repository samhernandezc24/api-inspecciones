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
            migrationBuilder.AlterColumn<int>(
                name: "Odometro",
                schema: "inspeccion",
                table: "Inspecciones",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Horometro",
                schema: "inspeccion",
                table: "Inspecciones",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Capacidad",
                schema: "inspeccion",
                table: "Inspecciones",
                type: "decimal(15,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,3)");

            migrationBuilder.AddColumn<string>(
                name: "IdUnidadTipo",
                schema: "inspeccion",
                table: "Inspecciones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnidadTipoName",
                schema: "inspeccion",
                table: "Inspecciones",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdUnidadTipo",
                schema: "inspeccion",
                table: "Inspecciones");

            migrationBuilder.DropColumn(
                name: "UnidadTipoName",
                schema: "inspeccion",
                table: "Inspecciones");

            migrationBuilder.AlterColumn<int>(
                name: "Odometro",
                schema: "inspeccion",
                table: "Inspecciones",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Horometro",
                schema: "inspeccion",
                table: "Inspecciones",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Capacidad",
                schema: "inspeccion",
                table: "Inspecciones",
                type: "decimal(15,3)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,3)",
                oldNullable: true);
        }
    }
}
