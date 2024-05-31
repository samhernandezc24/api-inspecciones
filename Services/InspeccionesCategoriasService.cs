using API.Inspecciones.Models;
using API.Inspecciones.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

            string idInspeccion     = Globals.ParseGuid(data.idInspeccion);
            bool isParcial          = Globals.ParseBool(data.isParcial);

            Inspeccion objInspeccion = await _inspeccionesService.Find(idInspeccion);

            objInspeccion.FechaInspeccionInicial        = Globals.DateTime(data.fechaInspeccionInicial);
            objInspeccion.FechaInspeccionInicialUpdate  = Globals.DateTime(data.fechaInspeccionInicial);
            objInspeccion.IdUserInspeccionInicial       = objUser.Id;
            objInspeccion.UserInspeccionInicialName     = objUser.Nombre;

            objInspeccion.FechaInspeccionFinal          = Globals.DateTime(data.fechaInspeccionFinal);
            objInspeccion.FechaInspeccionFinalUpdate    = Globals.DateTime(data.fechaInspeccionFinal);
            objInspeccion.IdUserInspeccionFinal         = objUser.Id;
            objInspeccion.UserInspeccionFinalName       = objUser.Nombre;

            if (!isParcial) { objInspeccion.Evaluado = true; }

            objInspeccion.FechaEvaluacion   = DateTime.Now;
            objInspeccion.SetUpdated(objUser);

            List<InspeccionCategoria> rangeCategoria            = new List<InspeccionCategoria>();
            List<InspeccionCategoriaValue> rangeCategoriaValue  = new List<InspeccionCategoriaValue>();

            foreach (var categoria in data.categorias)
            {
                InspeccionCategoria objCategoria = new InspeccionCategoria();

                objCategoria.IdInspeccionCategoria  = Guid.NewGuid().ToString();
                objCategoria.IdCategoria            = Globals.ParseGuid(categoria.idCategoria);
                objCategoria.CategoriaName          = Globals.ToUpper(categoria.name);
                objCategoria.IdInspeccion           = idInspeccion;
                objCategoria.SetCreated(objUser);
                rangeCategoria.Add(objCategoria);

                foreach (var item in categoria.categoriasItems)
                {
                    InspeccionCategoriaValue objValue = new InspeccionCategoriaValue();

                    objValue.IdInspeccionCategoriaValue     = Guid.NewGuid().ToString();
                    objValue.IdInspeccionCategoria          = objCategoria.IdInspeccionCategoria;
                    objValue.IdCategoriaItem                = Globals.ParseGuid(item.idCategoriaItem);
                    objValue.CategoriaItemName              = Globals.ToUpper(item.name);
                    objValue.IdFormularioTipo               = Globals.ParseGuid(item.idFormularioTipo);
                    objValue.FormularioTipoName             = Globals.ToUpper(item.formularioTipoName);
                    objValue.FormularioValor                = Globals.ToString(item.formularioValor);
                    objValue.Value                          = Globals.ToString(item.value);
                    objValue.Observaciones                  = Globals.ToString(item.observaciones);
                    objValue.SetCreated(objUser);
                    rangeCategoriaValue.Add(objValue);
                }
            }

            _context.InspeccionesCategorias.AddRange(rangeCategoria);
            _context.InspeccionesCategoriasValues.AddRange(rangeCategoriaValue);

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
                                                        NoAplica            = d.NoAplica,
                                                        Observaciones       = d.Observaciones,
                                                        HasObservaciones    = !string.IsNullOrEmpty(d.Observaciones),
                                                    }).ToList()
                            })
                            .ToListAsync<dynamic>();
        }
    }
}
