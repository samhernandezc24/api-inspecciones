using System.ComponentModel.DataAnnotations;
using Workcube.Generic;

namespace API.Inspecciones.Models
{
    public class InspeccionCategoria : UserCreated
    {
        [Key]
        public string IdInspeccionCategoria { get; set; }

        // CATEGORIA
        public virtual Categoria Categoria { get; set; }
        public string IdCategoria { get; set; }
        public string CategoriaName { get; set; }

        // INSPECCION
        public virtual Inspeccion Inspeccion { get; set; }
        public string IdInspeccion { get; set; }

        public virtual List<InspeccionCategoriaValue> InspeccionesCategoriasValues { get; set; }
    }
}
