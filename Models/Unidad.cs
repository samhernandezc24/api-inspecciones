using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Workcube.Generic;

namespace API.Inspecciones.Models
{
    public class Unidad : UserCreated
    {
        [Key]
        public string IdUnidad { get; set; }
        public string NumeroEconomico { get; set; }
        public string NumeroSerie { get; set; }
        public string Descripcion { get; set; }
        public string Modelo { get; set; }
        public string AnioEquipo { get; set; }

        // BASE
        public string IdBase { get; set; }
        public string BaseName { get; set; }

        // UNIDAD MARCA
        public string IdUnidadMarca { get; set; }
        public string UnidadMarcaName { get; set; }

        // UNIDAD TIPO
        public string IdUnidadTipo { get; set; }
        public string UnidadTipoName { get; set; }

        [Column(TypeName = "decimal(15,3)")]
        public decimal Capacidad { get; set; }
        public int Horometro { get; set; }
        public int Odometro { get; set; }
    }
}
