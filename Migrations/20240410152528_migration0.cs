using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Inspecciones.Migrations
{
    /// <inheritdoc />
    public partial class migration0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "inspeccion");

            migrationBuilder.CreateTable(
                name: "FormulariosTipos",
                schema: "inspeccion",
                columns: table => new
                {
                    IdFormularioTipo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulariosTipos", x => x.IdFormularioTipo);
                });

            migrationBuilder.CreateTable(
                name: "InspeccionesEstatus",
                schema: "inspeccion",
                columns: table => new
                {
                    IdInspeccionEstatus = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspeccionesEstatus", x => x.IdInspeccionEstatus);
                });

            migrationBuilder.CreateTable(
                name: "InspeccionesTipos",
                schema: "inspeccion",
                columns: table => new
                {
                    IdInspeccionTipo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    IdCreatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspeccionesTipos", x => x.IdInspeccionTipo);
                });

            migrationBuilder.CreateTable(
                name: "Unidades",
                schema: "inspeccion",
                columns: table => new
                {
                    IdUnidad = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumeroEconomico = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NumeroSerie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modelo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnioEquipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdBase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUnidadMarca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnidadMarcaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUnidadTipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnidadTipoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Capacidad = table.Column<decimal>(type: "decimal(15,3)", nullable: false),
                    Horometro = table.Column<int>(type: "int", nullable: false),
                    Odometro = table.Column<int>(type: "int", nullable: false),
                    IdCreatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unidades", x => x.IdUnidad);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                schema: "inspeccion",
                columns: table => new
                {
                    IdCategoria = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdInspeccionTipo = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InspeccionTipoCodigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InspeccionTipoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    IdCreatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.IdCategoria);
                    table.ForeignKey(
                        name: "FK_Categorias_InspeccionesTipos_IdInspeccionTipo",
                        column: x => x.IdInspeccionTipo,
                        principalSchema: "inspeccion",
                        principalTable: "InspeccionesTipos",
                        principalColumn: "IdInspeccionTipo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inspecciones",
                schema: "inspeccion",
                columns: table => new
                {
                    IdInspeccion = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Folio = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdBase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdInspeccionEstatus = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InspeccionEstatusName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdInspeccionTipo = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InspeccionTipoCodigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InspeccionTipoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaInspeccionInicial = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaInspeccionInicialUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUserInspeccionInicial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserInspeccionInicialName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaInspeccionFinal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaInspeccionFinalUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUserInspeccionFinal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserInspeccionFinalName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdRequerimiento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequerimientoFolio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUnidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnidadNumeroEconomico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUnidadTemporal = table.Column<bool>(type: "bit", nullable: false),
                    IdUnidadMarca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnidadMarcaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUnidadPlacaTipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnidadPlacaTipoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Placa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroSerie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modelo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnioEquipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Locacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoPlataforma = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Capacidad = table.Column<decimal>(type: "decimal(15,3)", nullable: false),
                    Odometro = table.Column<int>(type: "int", nullable: false),
                    Horometro = table.Column<int>(type: "int", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirmaOperador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirmaVerificador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCreatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspecciones", x => x.IdInspeccion);
                    table.ForeignKey(
                        name: "FK_Inspecciones_InspeccionesEstatus_IdInspeccionEstatus",
                        column: x => x.IdInspeccionEstatus,
                        principalSchema: "inspeccion",
                        principalTable: "InspeccionesEstatus",
                        principalColumn: "IdInspeccionEstatus",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inspecciones_InspeccionesTipos_IdInspeccionTipo",
                        column: x => x.IdInspeccionTipo,
                        principalSchema: "inspeccion",
                        principalTable: "InspeccionesTipos",
                        principalColumn: "IdInspeccionTipo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategoriasItems",
                schema: "inspeccion",
                columns: table => new
                {
                    IdCategoriaItem = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdInspeccionTipo = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InspeccionTipoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCategoria = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CategoriaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    IdFormularioTipo = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FormularioTipoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormularioValor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoAplica = table.Column<bool>(type: "bit", nullable: false),
                    IdCreatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasItems", x => x.IdCategoriaItem);
                    table.ForeignKey(
                        name: "FK_CategoriasItems_Categorias_IdCategoria",
                        column: x => x.IdCategoria,
                        principalSchema: "inspeccion",
                        principalTable: "Categorias",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategoriasItems_FormulariosTipos_IdFormularioTipo",
                        column: x => x.IdFormularioTipo,
                        principalSchema: "inspeccion",
                        principalTable: "FormulariosTipos",
                        principalColumn: "IdFormularioTipo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategoriasItems_InspeccionesTipos_IdInspeccionTipo",
                        column: x => x.IdInspeccionTipo,
                        principalSchema: "inspeccion",
                        principalTable: "InspeccionesTipos",
                        principalColumn: "IdInspeccionTipo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InspeccionesCategorias",
                schema: "inspeccion",
                columns: table => new
                {
                    IdInspeccionCategoria = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdInspeccion = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InspeccionFolio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCreatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspeccionesCategorias", x => x.IdInspeccionCategoria);
                    table.ForeignKey(
                        name: "FK_InspeccionesCategorias_Inspecciones_IdInspeccion",
                        column: x => x.IdInspeccion,
                        principalSchema: "inspeccion",
                        principalTable: "Inspecciones",
                        principalColumn: "IdInspeccion",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InspeccionesFicheros",
                schema: "inspeccion",
                columns: table => new
                {
                    IdInspeccionFichero = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdInspeccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InspeccionFolio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    IdCreatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspeccionesFicheros", x => x.IdInspeccionFichero);
                    table.ForeignKey(
                        name: "FK_InspeccionesFicheros_Inspecciones_IdInspeccionFichero",
                        column: x => x.IdInspeccionFichero,
                        principalSchema: "inspeccion",
                        principalTable: "Inspecciones",
                        principalColumn: "IdInspeccion",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InspeccionesCategoriasItems",
                schema: "inspeccion",
                columns: table => new
                {
                    IdInspeccionCategoriaItem = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdInspeccion = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InspeccionFolio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdInspeccionCategoria = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InspeccionCategoriaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdFormularioTipo = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FormularioTipoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormularioValor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoAplica = table.Column<bool>(type: "bit", nullable: false),
                    IdCreatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspeccionesCategoriasItems", x => x.IdInspeccionCategoriaItem);
                    table.ForeignKey(
                        name: "FK_InspeccionesCategoriasItems_FormulariosTipos_IdFormularioTipo",
                        column: x => x.IdFormularioTipo,
                        principalSchema: "inspeccion",
                        principalTable: "FormulariosTipos",
                        principalColumn: "IdFormularioTipo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspeccionesCategoriasItems_InspeccionesCategorias_IdInspeccionCategoria",
                        column: x => x.IdInspeccionCategoria,
                        principalSchema: "inspeccion",
                        principalTable: "InspeccionesCategorias",
                        principalColumn: "IdInspeccionCategoria",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspeccionesCategoriasItems_Inspecciones_IdInspeccion",
                        column: x => x.IdInspeccion,
                        principalSchema: "inspeccion",
                        principalTable: "Inspecciones",
                        principalColumn: "IdInspeccion",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_IdInspeccionTipo",
                schema: "inspeccion",
                table: "Categorias",
                column: "IdInspeccionTipo");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasItems_IdCategoria",
                schema: "inspeccion",
                table: "CategoriasItems",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasItems_IdFormularioTipo",
                schema: "inspeccion",
                table: "CategoriasItems",
                column: "IdFormularioTipo");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasItems_IdInspeccionTipo",
                schema: "inspeccion",
                table: "CategoriasItems",
                column: "IdInspeccionTipo");

            migrationBuilder.CreateIndex(
                name: "IX_Inspecciones_Folio",
                schema: "inspeccion",
                table: "Inspecciones",
                column: "Folio",
                unique: true,
                filter: "[Folio] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Inspecciones_IdInspeccionEstatus",
                schema: "inspeccion",
                table: "Inspecciones",
                column: "IdInspeccionEstatus");

            migrationBuilder.CreateIndex(
                name: "IX_Inspecciones_IdInspeccionTipo",
                schema: "inspeccion",
                table: "Inspecciones",
                column: "IdInspeccionTipo");

            migrationBuilder.CreateIndex(
                name: "IX_InspeccionesCategorias_IdInspeccion",
                schema: "inspeccion",
                table: "InspeccionesCategorias",
                column: "IdInspeccion");

            migrationBuilder.CreateIndex(
                name: "IX_InspeccionesCategoriasItems_IdFormularioTipo",
                schema: "inspeccion",
                table: "InspeccionesCategoriasItems",
                column: "IdFormularioTipo");

            migrationBuilder.CreateIndex(
                name: "IX_InspeccionesCategoriasItems_IdInspeccion",
                schema: "inspeccion",
                table: "InspeccionesCategoriasItems",
                column: "IdInspeccion");

            migrationBuilder.CreateIndex(
                name: "IX_InspeccionesCategoriasItems_IdInspeccionCategoria",
                schema: "inspeccion",
                table: "InspeccionesCategoriasItems",
                column: "IdInspeccionCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_InspeccionesTipos_Codigo",
                schema: "inspeccion",
                table: "InspeccionesTipos",
                column: "Codigo",
                unique: true,
                filter: "[Codigo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unidades_NumeroEconomico",
                schema: "inspeccion",
                table: "Unidades",
                column: "NumeroEconomico",
                unique: true,
                filter: "[NumeroEconomico] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoriasItems",
                schema: "inspeccion");

            migrationBuilder.DropTable(
                name: "InspeccionesCategoriasItems",
                schema: "inspeccion");

            migrationBuilder.DropTable(
                name: "InspeccionesFicheros",
                schema: "inspeccion");

            migrationBuilder.DropTable(
                name: "Unidades",
                schema: "inspeccion");

            migrationBuilder.DropTable(
                name: "Categorias",
                schema: "inspeccion");

            migrationBuilder.DropTable(
                name: "FormulariosTipos",
                schema: "inspeccion");

            migrationBuilder.DropTable(
                name: "InspeccionesCategorias",
                schema: "inspeccion");

            migrationBuilder.DropTable(
                name: "Inspecciones",
                schema: "inspeccion");

            migrationBuilder.DropTable(
                name: "InspeccionesEstatus",
                schema: "inspeccion");

            migrationBuilder.DropTable(
                name: "InspeccionesTipos",
                schema: "inspeccion");
        }
    }
}
