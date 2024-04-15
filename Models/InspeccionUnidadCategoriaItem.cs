using System.ComponentModel.DataAnnotations;
using Workcube.Generic;
using Workcube.Libraries;

namespace API.Inspecciones.Models
{
    public class InspeccionUnidadCategoriaItem : UserCreated
    {
        [Key]
        public string IdInspeccionUnidadCategoriaItem {  get; set; }
        public string Name {  get; set; }
        public string Descripcion { get; set; }

        // INSPECCION UNIDAD
        public string IdInspeccionUnidad { get; set; }
        public virtual InspeccionUnidad InspeccionUnidad { get; set; }
        public string InspeccionUnidadFolio { get; set; }

        // INSPECCION UNIDAD CATEGORIA
        public string IdInspeccionUnidadCategoria { get; set; }
        public virtual InspeccionUnidadCategoria InspeccionUnidadCategoria { get; set; }
        public string InspeccionUnidadCategoriaName { get; set; }
        
        // FORMULARIO TIPO
        public string IdFormularioTipo { get; set; }
        public virtual FormularioTipo FormularioTipo { get; set; }
        public string FormularioTipoName {  get; set; }

        public string FormularioValor {  get; set; }        

        public string Value {  get; set; }
        public string ValueAnterior {  get; set; }
    }
}
