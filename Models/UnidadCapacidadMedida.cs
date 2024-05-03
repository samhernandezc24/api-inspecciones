using System.ComponentModel.DataAnnotations;

namespace API.Inspecciones.Models
{
    public class UnidadCapacidadMedida
    {
        [Key]
        public string IdUnidadCapacidadMedida { get; set; }
        public string Name { get; set; }
        public int Orden { get; set; }
        public bool Deleted { get; set; }

        public virtual List<Unidad> Unidades { get; set; }
        public virtual List<Inspeccion> Inspecciones { get; set; }
    }
}
