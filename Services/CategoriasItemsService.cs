using System.Security.Claims;
using API.Inspecciones.Models;
using API.Inspecciones.Persistence;
using Microsoft.EntityFrameworkCore;
using Workcube.Interfaces;
using Workcube.Libraries;

namespace API.Inspecciones.Services
{
    public class CategoriasItemsService : IGlobal<CategoriaItem>
    {
        private readonly Context _context;

        public CategoriasItemsService(Context context)
        {
            _context = context;
        }

        public async Task Create(dynamic data, ClaimsPrincipal user)
        {
            var objTransaction = _context.Database.BeginTransaction();

            string idCategoria          = Globals.ParseGuid(data.idCategoria);
            bool categoriasItemsExist   = await _context.CategoriasItems.AnyAsync(x => x.IdCategoria == idCategoria && !x.Deleted);
            int categoriaItemNuevoOrden = 1;

            if (categoriasItemsExist)
            {
                int categoriaItemLastOrden = await _context.CategoriasItems.Where(x => x.IdCategoria == idCategoria && !x.Deleted).MaxAsync(x => (int?)x.Orden) ?? 0;
                categoriaItemNuevoOrden = categoriaItemLastOrden + 1;
            }

            // GUARDAR CATEGORÍA ITEM
            CategoriaItem objModel = new CategoriaItem();
            objModel.IdCategoriaItem        = Guid.NewGuid().ToString();
            objModel.Name                   = "Pregunta";
            objModel.IdInspeccionTipo       = Globals.ParseGuid(data.idInspeccionTipo);
            objModel.InspeccionTipoName     = Globals.ToUpper(data.inspeccionTipoName);
            objModel.IdCategoria            = Globals.ParseGuid(data.idCategoria);
            objModel.CategoriaName          = Globals.ToUpper(data.categoriaName);
            objModel.Orden                  = categoriaItemNuevoOrden;
            objModel.IdFormularioTipo       = "ea52bdfd-8af6-4f5a-b182-2b99e554eb32";
            objModel.FormularioTipoName     = "Opción múltiple";
            objModel.FormularioValor        = "Sí,No";
            objModel.NoAplica               = false;
            objModel.SetCreated(Globals.GetUser(user));

            _context.CategoriasItems.Add(objModel);

            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public async Task CreateDuplicate(dynamic data, ClaimsPrincipal user)
        {
            var objTransaction = _context.Database.BeginTransaction();

            string idCategoria          = Globals.ParseGuid(data.idCategoria);
            bool categoriasItemsExist   = await _context.CategoriasItems.AnyAsync(x => x.IdCategoria == idCategoria && !x.Deleted);
            int categoriaItemNuevoOrden = 1;

            if (categoriasItemsExist)
            {
                int categoriaItemLastOrden = await _context.CategoriasItems.Where(x => x.IdCategoria == idCategoria && !x.Deleted).MaxAsync(x => (int?)x.Orden) ?? 0;
                categoriaItemNuevoOrden = categoriaItemLastOrden + 1;
            }

            // GUARDAR CATEGORÍA ITEM DUPLICADA
            CategoriaItem objModel = new CategoriaItem();
            objModel.IdCategoriaItem        = Guid.NewGuid().ToString();
            objModel.Name                   = Globals.ToString(data.name);
            objModel.IdInspeccionTipo       = Globals.ParseGuid(data.idInspeccionTipo);
            objModel.InspeccionTipoName     = Globals.ToUpper(data.inspeccionTipoName);
            objModel.IdCategoria            = Globals.ParseGuid(data.idCategoria);
            objModel.CategoriaName          = Globals.ToUpper(data.categoriaName);
            objModel.Orden                  = categoriaItemNuevoOrden;
            objModel.IdFormularioTipo       = Globals.ParseGuid(data.idFormularioTipo);
            objModel.FormularioTipoName     = Globals.ToString(data.formularioTipoName);
            objModel.FormularioValor        = Globals.ToString(data.formularioValor);
            objModel.NoAplica               = false;
            objModel.SetCreated(Globals.GetUser(user));

            _context.CategoriasItems.Add(objModel);

            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public Task<dynamic> DataSource(dynamic data, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(dynamic data, ClaimsPrincipal user)
        {
             var objTransaction = _context.Database.BeginTransaction();

            string idCategoria      = Globals.ParseGuid(data.idCategoria);
            string idCategoriaItem  = Globals.ParseGuid(data.idCategoriaItem);

            // ENCONTRAR UNA CATEGORÍA ITEM PARA ELIMINAR
            CategoriaItem objModel = await Find(idCategoriaItem);

            if (objModel == null) { throw new ArgumentException("No se ha encontrado la pregunta solicitada."); }
            if (objModel.Deleted) { throw new ArgumentException("La pregunta ya fue eliminada anteriormente."); }

            // ELIMINAR CATEGORÍA ITEM
            objModel.Deleted    = true;
            objModel.Orden      = 0;
            objModel.SetUpdated(Globals.GetUser(user));

            _context.SaveChanges();

            var lstCategoriasItems = _context.CategoriasItems.OrderBy(x => x.Orden).Where(x => x.IdCategoria == idCategoria && !x.Deleted).ToList();

            int orden = 1;
            foreach (var item in lstCategoriasItems)
            {
                item.Orden = orden;
                orden++;
            }

            _context.CategoriasItems.Update(objModel);

            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public async Task<CategoriaItem> Find(string id)
        {
            return await _context.CategoriasItems.FindAsync(id);
        }

        public async Task<CategoriaItem> FindSelectorById(string id, string fields)
        {
            return await _context.CategoriasItems.AsNoTracking().Where(x => x.IdCategoriaItem == id)
                            .Select(Globals.BuildSelector<CategoriaItem, CategoriaItem>(fields)).FirstOrDefaultAsync();
        }

        public Task<List<dynamic>> List()
        {
            throw new NotImplementedException();
        }

        public async Task<List<dynamic>> List(string idCategoria)
        {
            return await _context.CategoriasItems
                            .AsNoTracking()
                            .Where(x => x.IdCategoria == idCategoria && !x.Deleted)
                            .OrderBy(x => x.Orden)
                            .Select(x => new
                            {
                                IdCategoriaItem     = x.IdCategoriaItem,
                                Name                = x.Name,
                                IdCategoria         = x.IdCategoria,
                                CategoriaName       = x.CategoriaName,
                                Orden               = x.Orden,
                                IdFormularioTipo    = string.IsNullOrEmpty(x.IdFormularioTipo) ? "" : x.IdFormularioTipo,
                                FormularioTipoName  = x.FormularioTipoName,
                                FormularioValor     = x.FormularioValor,
                                IsEdit              = false,
                            })
                            .ToListAsync<dynamic>();
        }

        public Task<byte[]> Reporte(dynamic data)
        {
            throw new NotImplementedException();
        }

        public async Task Update(dynamic data, ClaimsPrincipal user)
        {
            var objTransaction = _context.Database.BeginTransaction();

            string idCategoriaItem      = Globals.ParseGuid(data.idCategoriaItem);

            // ENCONTRAR UNA CATEGORÍA ITEM PARA ACTUALIZAR
            CategoriaItem objModel = await Find(idCategoriaItem);

            if (objModel == null) { throw new ArgumentException("No se ha encontrado la pregunta solicitada."); }
            if (objModel.Deleted) { throw new ArgumentException("La pregunta ya fue eliminada anteriormente."); }                     

            // ACTUALIZAR CATEGORÍA ITEM         
            objModel.Name                   = Globals.ToString(data.name);
            objModel.IdFormularioTipo       = Globals.ParseGuid(data.idFormularioTipo);
            objModel.FormularioTipoName     = Globals.ToString(data.formularioTipoName);
            objModel.FormularioValor        = Globals.ToString(data.formularioValor);
            objModel.NoAplica               = false;
            objModel.SetUpdated(Globals.GetUser(user));

            _context.CategoriasItems.Update(objModel);

            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }
    }
}
