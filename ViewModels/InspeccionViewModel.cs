namespace API.Inspecciones.ViewModels
{
    public class InspeccionViewModel
    {
        public string IdInspeccion { get; set; }
        public string Folio { get; set; }

        public DateTime Fecha { get; set; }
        public string FechaNatural => Fecha.ToString("dd/MM/yyyy hh:mm tt");

        // BASE
        public string IdBase { get; set; }
        public string BaseName { get; set; }

        // INSPECCION ESTATUS
        public string IdInspeccionEstatus { get; set; }
        public string InspeccionEstatusName { get; set; }

        // INSPECCION TIPO
        public string IdInspeccionTipo { get; set; }
        public string InspeccionTipoCodigo { get; set; }
        public string InspeccionTipoName { get; set; }

        // INSPECCION INICIAL
        public DateTime? FechaInspeccionInicial { get; set; }
        public string FechaInspeccionInicialNatural => FechaInspeccionInicial.HasValue ? FechaInspeccionInicial.Value.ToString("dd/MM/yyyy hh:mm tt").ToUpper() : "";
        public string UserInspeccionInicialName { get; set; }

        // INSPECCION FINAL
        public DateTime? FechaInspeccionFinal { get; set; }
        public string FechaInspeccionFinalNatural => FechaInspeccionFinal.HasValue ? FechaInspeccionFinal.Value.ToString("dd/MM/yyyy hh:mm tt").ToUpper() : ""; 
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

        // UNIDAD TIPO
        public string IdUnidadTipo { get; set; }
        public string UnidadTipoName { get; set; }

        // UNIDAD PLACA TIPO
        public string IdUnidadPlacaTipo { get; set; }
        public string UnidadPlacaTipoName { get; set; }

        public string Placa { get; set; }
        public string NumeroSerie { get; set; }
        public string Modelo { get; set; }
        public string AnioEquipo { get; set; }
        public string Locacion { get; set; }
        public string TipoPlataforma { get; set; }

        public decimal? Capacidad { get; set; }
        public int? Odometro { get; set; }
        public int? Horometro { get; set; }

        public string Observaciones { get; set; }
        public string FirmaOperador { get; set; }
        public string FirmaVerificador { get; set; }

        public string CreatedUserName { get; set; }
        public DateTime CreatedFecha { get; set; }
        public string CreatedFechaNatural => CreatedFecha.ToString("dd/MM/yyyy hh:mm tt");

        public string UpdatedUserName { get; set; }
        public DateTime UpdatedFecha { get; set; }
        public string UpdatedFechaNatural => UpdatedFecha.ToString("dd/MM/yyyy hh:mm tt");
    }
}
