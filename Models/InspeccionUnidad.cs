using System.ComponentModel.DataAnnotations;
using Workcube.Generic;

namespace API.Inspecciones.Models
{
    public class InspeccionUnidad : UserCreated
    {
        [Key]
        public string IdInspeccionUnidad {  get; set; }

        // BASE
        public string IdBase { get; set; }
        public string BaseName { get; set; }

        // UNIDAD INVENTARIO / UNIDAD TEMPORAL ??
        public string IdUnidad { get; set; }
        public string UnidadNumeroEconomico { get; set; }
        public bool IsUnidadTemporal {  get; set; }

        public string Folio {  get; set; }

        public DateTime Fecha {  get; set; }
        public string FechaNatural => this.Fecha.ToString("dd/MM/yyyy hh:mm:ss");

        public bool IsValid => (DateTime.Now.Date - this.Fecha.Date).TotalDays <= 15;

        // INSPECCION
        public string IdInspeccion {  get; set; }
        public virtual Inspeccion Inspeccion { get; set; }
        public string InspeccionFolio {  get; set; }
        public string InspeccionName {  get; set; }

        // INSPECCION DE UNIDAD INICIAL
        public DateTime? FechaInspeccionInicial { get; set; }
        public string FechaInspeccionInicialNatural => this.FechaInspeccionInicial.HasValue ? this.FechaInspeccionInicial.Value.ToString("dd DE MMMM DE yyyy").ToUpper() : "NA";
        public DateTime? FechaInspeccionInicialUpdate { get; set; }

        // INSPECCION DE UNIDAD FINAL
        public DateTime? FechaInspeccionFinal { get; set; }
        public string FechaInspeccionFinalNatural => this.FechaInspeccionFinal.HasValue ? this.FechaInspeccionFinal.Value.ToString("dd DE MMMM DE yyyy").ToUpper() : "NA";
        public DateTime? FechaInspeccionFinalUpdate { get; set; }

        // REQUERIMIENTO
        public string IdRequerimiento {  get; set; }
        public string RequerimientoFolio {  get; set; }
        public bool HasRequerimiento => !string.IsNullOrEmpty(this.RequerimientoFolio);
        
        public string TipoPlataforma {  get; set; }
        public string NumeroSerie {  get; set; }
        public string Marca {  get; set; }
        public string Modelo {  get; set; }
        public int Horometro {  get; set; }
        public int Odometro {  get; set; }
        public string Locacion {  get; set; }
        public int Capacidad {  get; set; }
        public string Observaciones {  get; set; }
        public string FirmaOperador {  get; set; }
        public string FirmaVerificador {  get; set; }

        public virtual List<InspeccionUnidadCategoria> InspeccionesUnidadesCategorias { get; set; }
        public virtual List<InspeccionUnidadCategoriaItem> InspeccionesUnidadesCategoriasItems { get; set; }
        public virtual List<InspeccionUnidadFichero> InspeccionesUnidadesFicheros { get; set; }
    }
}
