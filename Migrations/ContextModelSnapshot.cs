﻿// <auto-generated />
using System;
using API.Inspecciones.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Inspecciones.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("inspeccion")
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("API.Inspecciones.Models.Categoria", b =>
                {
                    b.Property<string>("IdCategoria")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedFecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("IdCreatedUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdInspeccionTipo")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdUpdatedUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InspeccionTipoCodigo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InspeccionTipoName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Orden")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedFecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdCategoria");

                    b.HasIndex("IdInspeccionTipo");

                    b.ToTable("Categorias", "inspeccion");
                });

            modelBuilder.Entity("API.Inspecciones.Models.CategoriaItem", b =>
                {
                    b.Property<string>("IdCategoriaItem")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoriaName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedFecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("FormularioTipoName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FormularioValor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdCategoria")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdCreatedUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdFormularioTipo")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdInspeccionTipo")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdUpdatedUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InspeccionTipoName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("NoAplica")
                        .HasColumnType("bit");

                    b.Property<int>("Orden")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedFecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdCategoriaItem");

                    b.HasIndex("IdCategoria");

                    b.HasIndex("IdFormularioTipo");

                    b.HasIndex("IdInspeccionTipo");

                    b.ToTable("CategoriasItems", "inspeccion");
                });

            modelBuilder.Entity("API.Inspecciones.Models.FormularioTipo", b =>
                {
                    b.Property<string>("IdFormularioTipo")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Orden")
                        .HasColumnType("int");

                    b.HasKey("IdFormularioTipo");

                    b.ToTable("FormulariosTipos", "inspeccion");
                });

            modelBuilder.Entity("API.Inspecciones.Models.Inspeccion", b =>
                {
                    b.Property<string>("IdInspeccion")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AnioEquipo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BaseName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Capacidad")
                        .HasColumnType("decimal(15,3)");

                    b.Property<DateTime>("CreatedFecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaInspeccionFinal")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaInspeccionFinalUpdate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaInspeccionInicial")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaInspeccionInicialUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirmaOperador")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirmaVerificador")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Folio")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Horometro")
                        .HasColumnType("int");

                    b.Property<string>("IdBase")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdCreatedUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdInspeccionEstatus")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdInspeccionTipo")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdRequerimiento")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdUnidad")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdUnidadMarca")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdUnidadPlacaTipo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdUpdatedUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdUserInspeccionFinal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdUserInspeccionInicial")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InspeccionEstatusName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InspeccionTipoCodigo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InspeccionTipoName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsUnidadTemporal")
                        .HasColumnType("bit");

                    b.Property<string>("Locacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Modelo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumeroSerie")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Observaciones")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Odometro")
                        .HasColumnType("int");

                    b.Property<string>("Placa")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequerimientoFolio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipoPlataforma")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UnidadMarcaName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UnidadNumeroEconomico")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UnidadPlacaTipoName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedFecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserInspeccionFinalName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserInspeccionInicialName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdInspeccion");

                    b.HasIndex("Folio")
                        .IsUnique()
                        .HasFilter("[Folio] IS NOT NULL");

                    b.HasIndex("IdInspeccionEstatus");

                    b.HasIndex("IdInspeccionTipo");

                    b.ToTable("Inspecciones", "inspeccion");
                });

            modelBuilder.Entity("API.Inspecciones.Models.InspeccionCategoria", b =>
                {
                    b.Property<string>("IdInspeccionCategoria")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedFecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("IdCreatedUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdInspeccion")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdUpdatedUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InspeccionFolio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedFecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdInspeccionCategoria");

                    b.HasIndex("IdInspeccion");

                    b.ToTable("InspeccionesCategorias", "inspeccion");
                });

            modelBuilder.Entity("API.Inspecciones.Models.InspeccionCategoriaItem", b =>
                {
                    b.Property<string>("IdInspeccionCategoriaItem")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedFecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("FormularioTipoName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FormularioValor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdCreatedUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdFormularioTipo")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdInspeccion")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdInspeccionCategoria")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdUpdatedUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InspeccionCategoriaName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InspeccionFolio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("NoAplica")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedFecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdInspeccionCategoriaItem");

                    b.HasIndex("IdFormularioTipo");

                    b.HasIndex("IdInspeccion");

                    b.HasIndex("IdInspeccionCategoria");

                    b.ToTable("InspeccionesCategoriasItems", "inspeccion");
                });

            modelBuilder.Entity("API.Inspecciones.Models.InspeccionEstatus", b =>
                {
                    b.Property<string>("IdInspeccionEstatus")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Orden")
                        .HasColumnType("int");

                    b.HasKey("IdInspeccionEstatus");

                    b.ToTable("InspeccionesEstatus", "inspeccion");
                });

            modelBuilder.Entity("API.Inspecciones.Models.InspeccionFichero", b =>
                {
                    b.Property<string>("IdInspeccionFichero")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedFecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("IdCreatedUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdInspeccion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdUpdatedUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InspeccionFolio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Orden")
                        .HasColumnType("int");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedFecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdInspeccionFichero");

                    b.ToTable("InspeccionesFicheros", "inspeccion");
                });

            modelBuilder.Entity("API.Inspecciones.Models.InspeccionTipo", b =>
                {
                    b.Property<string>("IdInspeccionTipo")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Codigo")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedFecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdCreatedUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdUpdatedUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Orden")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedFecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdInspeccionTipo");

                    b.HasIndex("Codigo")
                        .IsUnique()
                        .HasFilter("[Codigo] IS NOT NULL");

                    b.ToTable("InspeccionesTipos", "inspeccion");
                });

            modelBuilder.Entity("API.Inspecciones.Models.Unidad", b =>
                {
                    b.Property<string>("IdUnidad")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AnioEquipo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BaseName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Capacidad")
                        .HasColumnType("decimal(15,3)");

                    b.Property<DateTime>("CreatedFecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Horometro")
                        .HasColumnType("int");

                    b.Property<string>("IdBase")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdCreatedUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdUnidadMarca")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdUnidadTipo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdUpdatedUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Modelo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumeroEconomico")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NumeroSerie")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Odometro")
                        .HasColumnType("int");

                    b.Property<string>("UnidadMarcaName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UnidadTipoName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedFecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdUnidad");

                    b.HasIndex("NumeroEconomico")
                        .IsUnique()
                        .HasFilter("[NumeroEconomico] IS NOT NULL");

                    b.ToTable("Unidades", "inspeccion");
                });

            modelBuilder.Entity("API.Inspecciones.Models.Categoria", b =>
                {
                    b.HasOne("API.Inspecciones.Models.InspeccionTipo", "InspeccionTipo")
                        .WithMany("Categorias")
                        .HasForeignKey("IdInspeccionTipo")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("InspeccionTipo");
                });

            modelBuilder.Entity("API.Inspecciones.Models.CategoriaItem", b =>
                {
                    b.HasOne("API.Inspecciones.Models.Categoria", "Categoria")
                        .WithMany("CategoriasItems")
                        .HasForeignKey("IdCategoria")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("API.Inspecciones.Models.FormularioTipo", "FormularioTipo")
                        .WithMany("CategoriasItems")
                        .HasForeignKey("IdFormularioTipo")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("API.Inspecciones.Models.InspeccionTipo", "InspeccionTipo")
                        .WithMany("CategoriasItems")
                        .HasForeignKey("IdInspeccionTipo")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Categoria");

                    b.Navigation("FormularioTipo");

                    b.Navigation("InspeccionTipo");
                });

            modelBuilder.Entity("API.Inspecciones.Models.Inspeccion", b =>
                {
                    b.HasOne("API.Inspecciones.Models.InspeccionEstatus", "InspeccionEstatus")
                        .WithMany("Inspecciones")
                        .HasForeignKey("IdInspeccionEstatus")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("API.Inspecciones.Models.InspeccionTipo", "InspeccionTipo")
                        .WithMany("Inspecciones")
                        .HasForeignKey("IdInspeccionTipo")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("InspeccionEstatus");

                    b.Navigation("InspeccionTipo");
                });

            modelBuilder.Entity("API.Inspecciones.Models.InspeccionCategoria", b =>
                {
                    b.HasOne("API.Inspecciones.Models.Inspeccion", "Inspeccion")
                        .WithMany("InspeccionesCategorias")
                        .HasForeignKey("IdInspeccion")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Inspeccion");
                });

            modelBuilder.Entity("API.Inspecciones.Models.InspeccionCategoriaItem", b =>
                {
                    b.HasOne("API.Inspecciones.Models.FormularioTipo", "FormularioTipo")
                        .WithMany("InspeccionesCategoriasItems")
                        .HasForeignKey("IdFormularioTipo")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("API.Inspecciones.Models.Inspeccion", "Inspeccion")
                        .WithMany("InspeccionesCategoriasItems")
                        .HasForeignKey("IdInspeccion")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("API.Inspecciones.Models.InspeccionCategoria", "InspeccionCategoria")
                        .WithMany("InspeccionesCategoriasItems")
                        .HasForeignKey("IdInspeccionCategoria")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("FormularioTipo");

                    b.Navigation("Inspeccion");

                    b.Navigation("InspeccionCategoria");
                });

            modelBuilder.Entity("API.Inspecciones.Models.InspeccionFichero", b =>
                {
                    b.HasOne("API.Inspecciones.Models.Inspeccion", "Inspeccion")
                        .WithMany("InspeccionesFicheros")
                        .HasForeignKey("IdInspeccionFichero")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Inspeccion");
                });

            modelBuilder.Entity("API.Inspecciones.Models.Categoria", b =>
                {
                    b.Navigation("CategoriasItems");
                });

            modelBuilder.Entity("API.Inspecciones.Models.FormularioTipo", b =>
                {
                    b.Navigation("CategoriasItems");

                    b.Navigation("InspeccionesCategoriasItems");
                });

            modelBuilder.Entity("API.Inspecciones.Models.Inspeccion", b =>
                {
                    b.Navigation("InspeccionesCategorias");

                    b.Navigation("InspeccionesCategoriasItems");

                    b.Navigation("InspeccionesFicheros");
                });

            modelBuilder.Entity("API.Inspecciones.Models.InspeccionCategoria", b =>
                {
                    b.Navigation("InspeccionesCategoriasItems");
                });

            modelBuilder.Entity("API.Inspecciones.Models.InspeccionEstatus", b =>
                {
                    b.Navigation("Inspecciones");
                });

            modelBuilder.Entity("API.Inspecciones.Models.InspeccionTipo", b =>
                {
                    b.Navigation("Categorias");

                    b.Navigation("CategoriasItems");

                    b.Navigation("Inspecciones");
                });
#pragma warning restore 612, 618
        }
    }
}
