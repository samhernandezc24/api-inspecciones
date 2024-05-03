using System.ComponentModel.DataAnnotations;
using Workcube.Generic;

namespace API.Inspecciones.Models
{
    public class InspeccionCategoriaValue : UserCreated
    {
        [Key]
        public string IdInspeccionCategoriaValue { get; set; }

        // INSPECCION CATEGORIA
        public virtual InspeccionCategoria InspeccionCategoria { get; set; }
        public string IdInspeccionCategoria { get; set; }

        // CATEGORIA ITEM
        public virtual CategoriaItem CategoriaItem { get; set; }
        public string IdCategoriaItem { get; set; }
        public string CategoriaItemName { get; set; }

        // FORMULARIO TIPO
        public virtual FormularioTipo FormularioTipo { get; set; }
        public string IdFormularioTipo { get; set; }
        public string FormularioTipoName { get; set; }

        public string FormularioValor { get; set; }

        public string Value { get; set; }
    }
}
