using API.Inspecciones.Models;
using API.Inspecciones.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using Workcube.Libraries;

namespace API.Inspecciones.Services
{
    public class InspeccionesCategoriasService
    {
        private readonly Context _context;
        private readonly InspeccionesService _inspeccionesService;

        public InspeccionesCategoriasService(Context context, InspeccionesService inspeccionesService)
        {
            _context                = context;
            _inspeccionesService    = inspeccionesService;
        }

        public async Task<bool> Create(dynamic data, ClaimsPrincipal user)
        {
            var objUser = Globals.GetUser(user);
            var objTransaction = _context.Database.BeginTransaction();

            string idInspeccion = Globals.ParseGuid(data.idInspeccion);
            bool isParcial      = Globals.ParseBool(data.isParcial);

            // ACTUALIZAR VALORES DE LA INSPECCION
            Inspeccion objInspeccion = await _inspeccionesService.Find(idInspeccion);

            if (objInspeccion == null) { throw new ArgumentException("No se encontró la inspección."); }

            objInspeccion.IdInspeccionEstatus           = "ea52bdfd-8af6-4f5a-b182-2b99e554eb32";
            objInspeccion.InspeccionEstatusName         = "EVALUACIÓN";

            objInspeccion.FechaInspeccionInicial        = Globals.DateTime(data.fechaInspeccionInicial);
            objInspeccion.FechaInspeccionInicialUpdate  = Globals.DateTime(data.fechaInspeccionInicial);
            objInspeccion.IdUserInspeccionInicial       = objUser.Id;
            objInspeccion.UserInspeccionInicialName     = objUser.Nombre;            

            if (!isParcial) { objInspeccion.Evaluado = true; }

            if (objInspeccion.Evaluado)
            {
                objInspeccion.IdInspeccionEstatus   = "ea52bdfd-8af6-4f5a-b182-2b99e554eb33";
                objInspeccion.InspeccionEstatusName = "POR FINALIZAR";
            }

            objInspeccion.FechaEvaluacion = DateTime.Now;
            objInspeccion.SetUpdated(objUser);

            var lstCategorias = _context.InspeccionesCategorias.Where(x => x.IdInspeccion == idInspeccion).ToList();
            List<string> idsCategorias = lstCategorias.Select(x => x.IdInspeccionCategoria).ToList();
            var lstCategoriasValues = _context.InspeccionesCategoriasValues.Where(x => idsCategorias.Contains(x.IdInspeccionCategoria)).ToList();

            foreach (var categoria in data.categorias)
            {
                string idCategoria = Globals.ParseGuid(categoria.idCategoria);
                InspeccionCategoria objCategoria = lstCategorias.FirstOrDefault(x => x.IdCategoria == idCategoria);

                if (objCategoria == null)
                {
                    // GUARDAR INSPECCION CATEGORIA
                    objCategoria = new InspeccionCategoria();

                    objCategoria.IdInspeccionCategoria  = Guid.NewGuid().ToString();
                    objCategoria.IdCategoria            = Globals.ParseGuid(categoria.idCategoria);
                    objCategoria.CategoriaName          = Globals.ToUpper(categoria.name);
                    objCategoria.IdInspeccion           = idInspeccion;
                    objCategoria.SetCreated(objUser);

                    _context.InspeccionesCategorias.Add(objCategoria);
                }              

                foreach (var item in categoria.categoriasItems)
                {
                    string idCategoriaItem = Globals.ParseGuid(item.idCategoriaItem);
                    InspeccionCategoriaValue objValue = lstCategoriasValues.FirstOrDefault(x => x.IdCategoriaItem == idCategoriaItem);

                    if (objValue != null)
                    {
                        // ACTUALIZAR INSPECCION CATEGORIA VALUE
                        objValue.CategoriaItemName  = Globals.ToUpper(item.name);
                        objValue.IdFormularioTipo   = Globals.ParseGuid(item.idFormularioTipo);
                        objValue.FormularioTipoName = Globals.ToUpper(item.formularioTipoName);
                        objValue.FormularioValor    = Globals.ToString(item.formularioValor);
                        objValue.Value              = Globals.ToString(item.value);
                        objValue.NoAplica           = Globals.ParseBool(item.noAplica);
                        objValue.Observaciones      = Globals.ToUpper(item.observaciones);
                        objValue.SetUpdated(objUser);

                        _context.InspeccionesCategoriasValues.Update(objValue);
                    } 
                    else
                    {
                        // GUARDAR INSPECCION CATEGORIA VALUE
                        objValue = new InspeccionCategoriaValue();

                        objValue.IdInspeccionCategoriaValue     = Guid.NewGuid().ToString();
                        objValue.IdInspeccionCategoria          = objCategoria.IdInspeccionCategoria;
                        objValue.IdCategoriaItem                = Globals.ParseGuid(item.idCategoriaItem);
                        objValue.CategoriaItemName              = Globals.ToUpper(item.name);
                        objValue.IdFormularioTipo               = Globals.ParseGuid(item.idFormularioTipo);
                        objValue.FormularioTipoName             = Globals.ToUpper(item.formularioTipoName);
                        objValue.FormularioValor                = Globals.ToString(item.formularioValor);
                        objValue.Value                          = Globals.ToString(item.value);
                        objValue.NoAplica                       = Globals.ParseBool(item.noAplica);
                        objValue.Observaciones                  = Globals.ToUpper(item.observaciones);
                        objValue.SetCreated(objUser);

                        _context.InspeccionesCategoriasValues.Add(objValue);
                    }
                }
            }

            await _context.SaveChangesAsync();
            objTransaction.Commit();

            return isParcial;
        }

        public async Task<List<dynamic>> ListEvaluacion(string idInspeccion)
        {
            return await _context.InspeccionesCategorias
                            .AsNoTracking()
                            .Where(x => x.IdInspeccion == idInspeccion)
                            .OrderBy(x => x.CreatedFecha)
                            .Select(x => new
                            {
                                IdCategoria     = x.IdCategoria,
                                Name            = x.CategoriaName,
                                CategoriasItems = x.InspeccionesCategoriasValues
                                                    .OrderBy(d => d.CreatedFecha)
                                                    .Select(d => new
                                                    {
                                                        IdCategoriaItem     = d.IdCategoriaItem,
                                                        Name                = d.CategoriaItemName,
                                                        IdFormularioTipo    = d.IdFormularioTipo,
                                                        FormularioTipoName  = d.FormularioTipoName,
                                                        FormularioValor     = d.FormularioValor,
                                                        Value               = d.Value,
                                                        Observaciones       = d.Observaciones,
                                                        NoAplica            = d.NoAplica,
                                                    }).ToList()
                            })
                            .ToListAsync<dynamic>();
        }
    }
}
