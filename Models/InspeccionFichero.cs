using System.ComponentModel.DataAnnotations;
using Workcube.Generic;

namespace API.Inspecciones.Models
{
    public class InspeccionFichero : UserCreated
    {
        [Key]
        public string IdInspeccionFichero { get; set; }

        public string Path {  get; set; }

        // INSPECCION
        public virtual Inspeccion Inspeccion { get; set; }
        public string IdInspeccion { get; set; }
    }
}
