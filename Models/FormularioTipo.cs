using System.ComponentModel.DataAnnotations;

namespace API.Inspecciones.Models
{
    public class FormularioTipo
    {
        [Key]
        public string IdFormularioTipo { get; set; }
        public string Name {  get; set; }
        public int Orden {  get; set; }
        public bool Deleted {  get; set; }

        public virtual List<CategoriaItem> CategoriasItems { get; set; }
        public virtual List<InspeccionCategoriaValue> InspeccionesCategoriasValues { get; set; }
    }
}
