namespace API.Inspecciones.ViewModels
{
    public class UnidadViewModel
    {
        public string IdUnidad { get; set; }
        public string NumeroEconomico { get; set; }

        // BASE
        public string IdBase { get; set; }
        public string BaseName { get; set; }

        // UNIDAD TIPO
        public string IdUnidadTipo { get; set; }
        public string UnidadTipoName { get; set; }

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
        public string Descripcion { get; set; }

        public string Capacidad { get; set; }

        // UNIDAD CAPACIDAD MEDIDA
        public string IdUnidadCapacidadMedida { get; set; }
        public string UnidadCapacidadMedidaName { get; set; }
        public string CapacidadMedida { get; set; }

        public int? Horometro { get; set; }
        public int? Odometro { get; set; }

        public string CreatedUserName { get; set; }
        public DateTime CreatedFecha { get; set; }
        public string CreatedFechaNatural => CreatedFecha.ToString("dd/MM/yyyy hh:mm tt");

        public string UpdatedUserName { get; set; }
        public DateTime UpdatedFecha { get; set; }
        public string UpdatedFechaNatural => UpdatedFecha.ToString("dd/MM/yyyy hh:mm tt");
    }
}
