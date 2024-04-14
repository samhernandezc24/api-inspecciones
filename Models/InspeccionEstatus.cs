using System.ComponentModel.DataAnnotations;

namespace API.Inspecciones.Models
{
    public class InspeccionEstatus
    {
        [Key]
        public string IdInspeccionEstatus { get; set; }
        public string Name { get; set; }
        public int Orden {  get; set; }
        public bool Deleted {  get; set; }

        public virtual List<Inspeccion> Inspecciones { get; set; }
    }
}
