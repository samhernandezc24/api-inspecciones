using API.Inspecciones.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Inspecciones.Persistence
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("inspeccion");

            // CATEGORIAS
            modelBuilder.Entity<Categoria>().HasOne(item => item.InspeccionTipo).WithMany(item => item.Categorias).HasForeignKey(item => item.IdInspeccionTipo).OnDelete(DeleteBehavior.Restrict);

            // CATEGORIAS ITEMS
            modelBuilder.Entity<CategoriaItem>().HasOne(item => item.Categoria).WithMany(item => item.CategoriasItems).HasForeignKey(item => item.IdCategoria).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CategoriaItem>().HasOne(item => item.FormularioTipo).WithMany(item => item.CategoriasItems).HasForeignKey(item => item.IdFormularioTipo).OnDelete(DeleteBehavior.Restrict);

            // INSPECCIONES
            modelBuilder.Entity<Inspeccion>().HasIndex(item => item.Folio).IsUnique();
            modelBuilder.Entity<Inspeccion>().HasOne(item => item.InspeccionEstatus).WithMany(item => item.Inspecciones).HasForeignKey(item => item.IdInspeccionEstatus).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Inspeccion>().HasOne(item => item.InspeccionTipo).WithMany(item => item.Inspecciones).HasForeignKey(item => item.IdInspeccionTipo).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Inspeccion>().HasOne(item => item.UnidadCapacidadMedida).WithMany(item => item.Inspecciones).HasForeignKey(item => item.IdUnidadCapacidadMedida).OnDelete(DeleteBehavior.Restrict);

            // INSPECCIONES CATEGORIAS
            modelBuilder.Entity<InspeccionCategoria>().HasOne(item => item.Inspeccion).WithMany(item => item.InspeccionesCategorias).HasForeignKey(item => item.IdInspeccion).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InspeccionCategoria>().HasOne(item => item.Categoria).WithMany(item => item.InspeccionesCategorias).HasForeignKey(item => item.IdCategoria).OnDelete(DeleteBehavior.Restrict);

            // INSPECCIONES CATEGORIAS VALUES
            modelBuilder.Entity<InspeccionCategoriaValue>().HasOne(item => item.InspeccionCategoria).WithMany(item => item.InspeccionesCategoriasValues).HasForeignKey(item => item.IdInspeccionCategoria).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InspeccionCategoriaValue>().HasOne(item => item.CategoriaItem).WithMany(item => item.InspeccionesCategoriasValues).HasForeignKey(item => item.IdCategoriaItem).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InspeccionCategoriaValue>().HasOne(item => item.FormularioTipo).WithMany(item => item.InspeccionesCategoriasValues).HasForeignKey(item => item.IdFormularioTipo).OnDelete(DeleteBehavior.Restrict);

            // INSPECCIONES FICHEROS
            modelBuilder.Entity<InspeccionFichero>().HasOne(item => item.Inspeccion).WithMany(item => item.InspeccionesFicheros).HasForeignKey(item => item.IdInspeccion).OnDelete(DeleteBehavior.Restrict);

            // INSPECCIONES TIPOS
            modelBuilder.Entity<InspeccionTipo>().HasIndex(item => item.Codigo).IsUnique();

            // UNIDADES (TEMPORALES)
            modelBuilder.Entity<Unidad>().HasIndex(item => item.NumeroEconomico).IsUnique();
            modelBuilder.Entity<Unidad>().HasOne(item => item.UnidadCapacidadMedida).WithMany(item => item.Unidades).HasForeignKey(item => item.IdUnidadCapacidadMedida).OnDelete(DeleteBehavior.Restrict);
        }

        // C
        public virtual DbSet<Categoria> Categorias { get; set; }
        public virtual DbSet<CategoriaItem> CategoriasItems { get; set; }

        // F
        public virtual DbSet<FormularioTipo> FormulariosTipos { get; set; }

        // I
        public virtual DbSet<Inspeccion> Inspecciones {  get; set; }
        public virtual DbSet<InspeccionEstatus> InspeccionesEstatus {  get; set; }
        public virtual DbSet<InspeccionCategoria> InspeccionesCategorias { get; set; }
        public virtual DbSet<InspeccionCategoriaValue> InspeccionesCategoriasValues { get; set; }
        public virtual DbSet<InspeccionFichero> InspeccionesFicheros { get; set; }
        public virtual DbSet<InspeccionTipo> InspeccionesTipos { get; set; }

        // U
        public virtual DbSet<Unidad> Unidades { get; set; }
        public virtual DbSet<UnidadCapacidadMedida> UnidadesCapacidadesMedidas { get; set; }
    }
}
