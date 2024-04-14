namespace API.Inspecciones.ViewModels
{
    public class UnidadViewModel
    {
        public string IdUnidad { get; set; }
        public string NumeroEconomico { get; set; }
        public string NumeroSerie { get; set; }
        public string Descripcion { get; set; }
        public string Modelo { get; set; }
        public string AnioEquipo { get; set; }

        // UNIDAD MARCA
        public string IdUnidadMarca { get; set; }
        public string UnidadMarcaName { get; set; }

        // UNIDAD TIPO
        public string IdUnidadTipo { get; set; }
        public string UnidadTipoName { get; set; }

        public string Capacidad { get; set; }

        public string CreatedUserName {  get; set; }
        public DateTime CreatedFecha { get; set; }
        public string CreatedFechaNatural => CreatedFecha.ToString("dd/MM/yyyy hh:mm tt");

        public string UpdatedUserName { get; set; }
        public DateTime UpdatedFecha { get; set; }
        public string UpdatedFechaNatural => UpdatedFecha.ToString("dd/MM/yyyy hh:mm tt");
    }
}
