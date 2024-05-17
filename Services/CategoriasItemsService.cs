using API.Inspecciones.Models;
using API.Inspecciones.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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

            string idCategoria = Globals.ParseGuid(data.idCategoria);

            // Verificar si coincide el id de la categoria.
            var findCategoria = await _context.Categorias.AnyAsync(x => x.IdCategoria == idCategoria && !x.Deleted);
            if (!findCategoria) { throw new ArgumentException("La categoria especificada no existe."); }

            // Obtener todas las preguntas existentes para la categoria dada.
            var lstCategoriasItems = await _context.CategoriasItems
                                        .Where(x => x.IdCategoria == idCategoria && !x.Deleted)
                                        .OrderBy(x => x.Orden)
                                        .ToListAsync();

            // Determinar el nuevo valor de orden para la nueva pregunta.
            int newOrdenValue = lstCategoriasItems.Count > 0 ? lstCategoriasItems.Max(x => x.Orden) + 1 : 1;

            // GUARDAR CATEGORIA ITEM
            CategoriaItem objModel = new CategoriaItem();

            objModel.IdCategoriaItem        = Guid.NewGuid().ToString();
            objModel.Name                   = "Pregunta";
            objModel.IdCategoria            = idCategoria;
            objModel.CategoriaName          = Globals.ToUpper(data.categoriaName);
            objModel.IdFormularioTipo       = "ea52bdfd-8af6-4f5a-b182-2b99e554eb32";
            objModel.FormularioTipoName     = "Opción múltiple";
            objModel.Orden                  = newOrdenValue;
            objModel.FormularioValor        = "Sí,No";
            objModel.SetCreated(Globals.GetUser(user));

            _context.CategoriasItems.Add(objModel);
            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public async Task CreateDuplicate(dynamic data, ClaimsPrincipal user)
        {
            var objTransaction = _context.Database.BeginTransaction();

            string idCategoria = Globals.ParseGuid(data.idCategoria);

            // Obtener todas las preguntas existentes para la categoria dada.
            var lstCategoriasItems = await _context.CategoriasItems
                                        .Where(x => x.IdCategoria == idCategoria && !x.Deleted)
                                        .OrderBy(x => x.Orden)
                                        .ToListAsync();

            // Determinar el nuevo valor de orden para la nueva pregunta.
            int newOrdenValue = lstCategoriasItems.Count > 0 ? lstCategoriasItems.Max(x => x.Orden) + 1 : 1;

            // DUPLICAR CATEGORIA ITEM
            CategoriaItem objModel = new CategoriaItem();

            objModel.IdCategoriaItem        = Guid.NewGuid().ToString();
            objModel.Name                   = Globals.ToString(data.name);
            objModel.IdCategoria            = idCategoria;
            objModel.CategoriaName          = Globals.ToUpper(data.categoriaName);
            objModel.IdFormularioTipo       = Globals.ParseGuid(data.idFormularioTipo);
            objModel.FormularioTipoName     = Globals.ToString(data.formularioTipoName);
            objModel.Orden                  = newOrdenValue;
            objModel.FormularioValor        = Globals.ToString(data.formularioValor);
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

            // ENCONTRAR CATEGORIA ITEM A ELIMINAR
            CategoriaItem objModel = await Find(idCategoriaItem);

            if (objModel == null) { throw new ArgumentException("No se encontró la pregunta."); }
            if (objModel.Deleted) { throw new ArgumentException("La pregunta ya fue eliminada anteriormente."); }

            // ELIMINAR CATEGORIA ITEM
            objModel.Deleted = true;
            objModel.Orden = 0;
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
                                IdFormularioTipo    = string.IsNullOrEmpty(x.IdFormularioTipo) ? "" : x.IdFormularioTipo,
                                FormularioTipoName  = x.FormularioTipoName,
                                Orden               = x.Orden,
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

            string idCategoriaItem  = Globals.ParseGuid(data.idCategoriaItem);

            // ENCONTRAR CATEGORIA ITEM A ACTUALIZAR
            CategoriaItem objModel = await Find(idCategoriaItem);

            if (objModel == null) { throw new ArgumentException("No se encontró la pregunta."); }
            if (objModel.Deleted) { throw new ArgumentException("La pregunta ha sido eliminada."); }

            // ACTUALIZAR CATEGORIA ITEM
            objModel.Name                   = Globals.ToString(data.name);
            objModel.IdFormularioTipo       = Globals.ParseGuid(data.idFormularioTipo);
            objModel.FormularioTipoName     = Globals.ToString(data.formularioTipoName);
            objModel.FormularioValor        = Globals.ToString(data.formularioValor);
            objModel.SetUpdated(Globals.GetUser(user));

            _context.CategoriasItems.Update(objModel);
            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }
    }
}