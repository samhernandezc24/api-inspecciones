using System.ComponentModel.DataAnnotations;
using Workcube.Generic;

namespace API.Inspecciones.Models
{
    public class InspeccionUnidadFichero : UserCreated
    {
        [Key]
        public string IdInspeccionUnidadFichero {  get; set; }

        // INSPECCION UNIDAD
        public string IdInspeccionUnidad {  get; set; }
        public virtual InspeccionUnidad InspeccionUnidad { get; set; }
        public string InspeccionUnidadFolio { get; set; }

        public string Path { get; set; }
        public int Orden {  get; set; }      
    }
}
