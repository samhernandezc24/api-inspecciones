using System.ComponentModel.DataAnnotations;
using Workcube.Generic;

namespace API.Inspecciones.Models
{
    public class InspeccionUnidadCategoria : UserCreated
    {
        [Key]
        public string IdInspeccionUnidadCategoria {  get; set; }
        public string Name {  get; set; }

        // INSPECCION UNIDAD
        public string IdInspeccionUnidad { get; set; }
        public virtual InspeccionUnidad InspeccionUnidad { get; set; }
        public string InspeccionUnidadFolio { get; set; }

        public virtual List<InspeccionUnidadCategoriaItem> InspeccionesUnidadesCategoriasItems { get; set; }
    }
}
