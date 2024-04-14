using System.ComponentModel.DataAnnotations;
using Workcube.Generic;
using Workcube.Libraries;

namespace API.Inspecciones.Models
{
    public class InspeccionCategoriaItem : UserCreated
    {
        [Key]
        public string IdInspeccionCategoriaItem { get; set; }
        public string Name { get; set; }
        
        // INSPECCION
        public virtual Inspeccion Inspeccion { get; set; }
        public string IdInspeccion { get; set; }
        public string InspeccionFolio { get; set; }

        // INSPECCION CATEGORIA
        public virtual InspeccionCategoria InspeccionCategoria { get; set; }
        public string IdInspeccionCategoria { get; set; }
        public string InspeccionCategoriaName { get; set; }

        // FORMULARIO TIPO
        public virtual FormularioTipo FormularioTipo { get; set; }
        public string IdFormularioTipo { get; set; }
        public string FormularioTipoName { get; set; }

        public string FormularioValor { get; set; }
        public string Value { get; set; }

        public dynamic ValueDynamic => IdFormularioTipo == "ea52bdfd-8af6-4f5a-b182-2b99e554eb34" ? Globals.ParseBool(Value) : Value;

        public string ValueReporte { get 
            {
                string value = Value;

                switch (Value)
                {
                    case "no_aplica":
                        value = "No aplica";
                        break;
                    case "malo":
                        value = "Malo";
                        break;
                    case "regular":
                        value = "Regular";
                        break;
                    case "bueno":
                        value = "Bueno";
                        break;
                    case "True":
                        value = Value == "True" ? "Sí" : "No";
                        break;
                }

                return value;            
            }
        }

        public bool NoAplica { get; set; }
    }
}
