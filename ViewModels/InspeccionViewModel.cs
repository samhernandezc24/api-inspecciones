namespace API.Inspecciones.ViewModels
{
    public class InspeccionViewModel
    {
        public string IdInspeccion { get; set; }

        // REQUERIMIENTO
        public string RequerimientoFolio { get; set; }
        public bool HasRequerimiento => !string.IsNullOrEmpty(RequerimientoFolio);

        public string Folio { get; set; }

        // FECHA PROGRAMADA
        public DateTime FechaProgramada { get; set; }
        public string FechaProgramadaNatural => FechaProgramada.ToString("dd/MM/yyyy hh:mm tt");

        // INSPECCIÓN INICIAL
        public DateTime? FechaInspeccionInicial { get; set; }
        public string FechaInspeccionInicialNatural => FechaInspeccionInicial.HasValue ? FechaInspeccionInicial.Value.ToString("dd/MM/yyyy hh:mm tt").ToUpper() : "";
        public string UserInspeccionInicialName { get; set; }

        // INSPECCION FINAL
        public DateTime? FechaInspeccionFinal { get; set; }
        public string FechaInspeccionFinalNatural => FechaInspeccionFinal.HasValue ? FechaInspeccionFinal.Value.ToString("dd/MM/yyyy hh:mm tt").ToUpper() : ""; 
        public string UserInspeccionFinalName { get; set; }

        public bool IsValid => FechaInspeccionFinal.HasValue && ((DateTime.Now.Date - FechaInspeccionFinal.Value.Date).TotalDays <= 15);

        // INSPECCION ESTATUS
        public string IdInspeccionEstatus { get; set; }
        public string InspeccionEstatusName { get; set; }

        // INSPECCION TIPO
        public string InspeccionTipoCodigo { get; set; }
        public string InspeccionTipoName { get; set; }

        // BASE
        public string BaseName { get; set; }

        // UNIDAD INVENTARIO / UNIDAD TEMPORAL
        public string IdUnidad { get; set; }
        public string UnidadNumeroEconomico { get; set; }
        public bool IsUnidadTemporal { get; set; }

        // UNIDAD TIPO
        public string UnidadTipoName { get; set; }

        // UNIDAD MARCA
        public string UnidadMarcaName { get; set; }

        // UNIDAD PLACA TIPO
        public string UnidadPlacaTipoName { get; set; }
        public string Placa { get; set; }

        public string NumeroSerie { get; set; }
        public string Modelo { get; set; }
        public string AnioEquipo { get; set; }

        public string Capacidad { get; set; }

        // UNIDAD CAPACIDAD MEDIDA
        public string UnidadCapacidadMedidaName { get; set; }

        // DATOS DE EVALUACIÓN
        public bool Evaluado { get; set; }
        public DateTime? FechaEvaluacion { get; set; }
        public string FechaEvaluacionNatural => FechaEvaluacion.HasValue ? FechaEvaluacion.Value.ToString("dd/MM/yyyy hh:mm tt") : "";

        public string Locacion { get; set; }
        public string TipoPlataforma { get; set; }

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
