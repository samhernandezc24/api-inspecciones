using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Workcube.Generic;

namespace API.Inspecciones.Models
{
    public class Inspeccion : UserCreated
    {
        [Key]
        public string IdInspeccion { get; set; }
        public string Folio { get; set; }

        public DateTime Fecha { get; set; }
        public string FechaNatural => Fecha.ToString("dd/MM/yyyy hh:mm tt");

        // BASE
        public string IdBase { get; set; }
        public string BaseName { get; set; }

        // INSPECCION ESTATUS
        public virtual InspeccionEstatus InspeccionEstatus { get; set; }
        public string IdInspeccionEstatus { get; set; }
        public string InspeccionEstatusName { get; set; }

        // INSPECCION TIPO
        public virtual InspeccionTipo InspeccionTipo { get; set; }
        public string IdInspeccionTipo { get; set; }
        public string InspeccionTipoCodigo { get; set; }
        public string InspeccionTipoName { get; set; }

        // INSPECCION INICIAL
        public DateTime? FechaInspeccionInicial { get; set; }
        public string FechaInspeccionInicialNatural => FechaInspeccionInicial.HasValue ? FechaInspeccionInicial.Value.ToString("dd DE MMMM DE yyyy A LAS hh:mm tt").ToUpper() : "NA";
        public DateTime? FechaInspeccionInicialUpdate { get; set; }
        public string IdUserInspeccionInicial { get; set; }
        public string UserInspeccionInicialName { get; set; }

        // INSPECCION FINAL
        public DateTime? FechaInspeccionFinal { get; set; }
        public string FechaInspeccionFinalNatural => FechaInspeccionFinal.HasValue ? FechaInspeccionFinal.Value.ToString("dd DE MMMM DE yyyy A LAS hh:mm tt").ToUpper() : "NA"; public DateTime? FechaInspeccionFinalUpdate { get; set; }
        public string IdUserInspeccionFinal { get; set; }
        public string UserInspeccionFinalName { get; set; }

        public bool IsValid => FechaInspeccionFinal.HasValue && ((DateTime.Now.Date - FechaInspeccionFinal.Value.Date).TotalDays <= 15);

        // REQUERIMIENTO
        public string IdRequerimiento { get; set; }
        public string RequerimientoFolio { get; set; }
        public bool HasRequerimiento => !string.IsNullOrEmpty(RequerimientoFolio);

        // UNIDAD INVENTARIO / UNIDAD TEMPORAL
        public string IdUnidad { get; set; }
        public string UnidadNumeroEconomico { get; set; }
        public bool IsUnidadTemporal { get; set; }

        // UNIDAD MARCA
        public string IdUnidadMarca { get; set; }
        public string UnidadMarcaName { get; set; }

        // UNIDAD PLACA TIPO
        public string IdUnidadPlacaTipo { get; set; }
        public string UnidadPlacaTipoName { get; set; }

        public string Placa { get; set; }
        public string NumeroSerie { get; set; }
        public string Modelo { get; set; }
        public string AnioEquipo { get; set; }
        public string Locacion { get; set; }
        public string TipoPlataforma { get; set; }

        [Column(TypeName = "decimal(15,3)")]
        public decimal Capacidad { get; set; }
        public int Odometro { get; set; }
        public int Horometro { get; set; }

        public string Observaciones { get; set; }
        public string FirmaOperador { get; set; }
        public string FirmaVerificador { get; set; }

        public virtual List<InspeccionCategoria> InspeccionesCategorias { get; set; }
        public virtual List<InspeccionCategoriaItem> InspeccionesCategoriasItems { get; set; }
        public virtual List<InspeccionFichero> InspeccionesFicheros { get; set; }
    }
}
