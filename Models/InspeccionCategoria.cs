using System.ComponentModel.DataAnnotations;
using Workcube.Generic;

namespace API.Inspecciones.Models
{
    public class InspeccionCategoria : UserCreated
    {
        [Key]
        public string IdInspeccionCategoria { get; set; }
        public string Name { get; set; }

        // INSPECCION
        public virtual Inspeccion Inspeccion { get; set; }
        public string IdInspeccion { get; set; }
        public string InspeccionFolio { get; set; }

        public virtual List<InspeccionCategoriaItem> InspeccionesCategoriasItems { get; set; }
    }
}
