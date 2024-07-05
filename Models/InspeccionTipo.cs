using System.ComponentModel.DataAnnotations;
using Workcube.Generic;

namespace API.Inspecciones.Models
{
    public class InspeccionTipo : UserCreated
    {
        [Key]
        public string IdInspeccionTipo { get; set; }

        public string Codigo { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public virtual List<Categoria> Categorias { get; set; }
        public virtual List<Inspeccion> Inspecciones { get; set; }
    }
}
