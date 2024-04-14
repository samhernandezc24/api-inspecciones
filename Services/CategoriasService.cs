using API.Inspecciones.Models;
using API.Inspecciones.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Workcube.Interfaces;
using Workcube.Libraries;

namespace API.Inspecciones.Services
{
    public class CategoriasService : IGlobal<Categoria>
    {
        private readonly Context _context;

        public CategoriasService(Context context)
        {
            _context = context;
        }

        public async Task Create(dynamic data, ClaimsPrincipal user)
        {
            var objTransaction = _context.Database.BeginTransaction();

            string idInspeccionTipo = Globals.ParseGuid(data.idInspeccionTipo);
            string categoriaName    = Globals.ToUpper(data.name);

            bool categoriasExist    = await _context.Categorias.AnyAsync(x => x.IdInspeccionTipo == idInspeccionTipo && !x.Deleted);

            int categoriaNuevoOrden = 1;
            if (categoriasExist)
            {
                int categoriaLastOrden = await _context.Categorias.Where(x => x.IdInspeccionTipo == idInspeccionTipo && !x.Deleted).MaxAsync(x => (int?)x.Orden) ?? 0;
                categoriaNuevoOrden = categoriaLastOrden + 1;
            }

            bool findCategoriaName  = await _context.Categorias.AnyAsync(x => x.Name.ToUpper() == categoriaName && x.IdInspeccionTipo == idInspeccionTipo && !x.Deleted);
            if (findCategoriaName) { throw new ArgumentException("Ya existe una categoría con ese nombre."); }

            // GUARDAR CATEGORÍA
            Categoria objModel = new Categoria();
            objModel.IdCategoria            = Guid.NewGuid().ToString();
            objModel.Name                   = categoriaName;
            objModel.IdInspeccionTipo       = idInspeccionTipo;
            objModel.InspeccionTipoCodigo   = Globals.ToUpper(data.inspeccionTipoCodigo);
            objModel.InspeccionTipoName     = Globals.ToUpper(data.inspeccionTipoName);
            objModel.Orden                  = categoriaNuevoOrden;
            objModel.SetCreated(Globals.GetUser(user));

            _context.Categorias.Add(objModel);

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

            string idInspeccionTipo = Globals.ParseGuid(data.idInspeccionTipo);
            string idCategoria      = Globals.ParseGuid(data.idCategoria);

            // ENCONTRAR UNA CATEGORÍA PARA ELIMINAR
            Categoria objModel = await Find(idCategoria);

            if (objModel == null) { throw new ArgumentException("No se ha encontrado la categoría solicitada."); }
            if (objModel.Deleted) { throw new ArgumentException("La categoría ya fue eliminada anteriormente."); }

            // ELIMINAR CATEGORÍA
            objModel.Deleted    = true;
            objModel.Orden      = 0;
            objModel.SetUpdated(Globals.GetUser(user));

            _context.SaveChanges();

            var lstCategorias = _context.Categorias.OrderBy(x => x.Orden).Where(x => x.IdInspeccionTipo == idInspeccionTipo && !x.Deleted).ToList();

            int orden = 1;
            foreach (var item in lstCategorias)
            {
                item.Orden = orden;
                orden++;
            }

            _context.Categorias.Update(objModel);

            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public async Task<Categoria> Find(string id)
        {
            return await _context.Categorias.FindAsync(id);
        }

        public async Task<Categoria> FindSelectorById(string id, string fields)
        {
            return await _context.Categorias.AsNoTracking().Where(x => x.IdCategoria == id)
                        .Select(Globals.BuildSelector<Categoria, Categoria>(fields)).FirstOrDefaultAsync();
        }

        public Task<List<dynamic>> List()
        {
            throw new NotImplementedException();
        }

        public async Task<List<dynamic>> List(string idInspeccionTipo)
        {
            return await _context.Categorias
                            .AsNoTracking()
                            .Where(x => x.IdInspeccionTipo == idInspeccionTipo && !x.Deleted)
                            .OrderBy(x => x.Orden)
                            .Select(x => new
                            {
                                IdCategoria             = x.IdCategoria,
                                Name                    = x.Name,
                                IdInspeccionTipo        = x.IdInspeccionTipo,
                                InspeccionTipoCodigo    = x.InspeccionTipoCodigo,
                                InspeccionTipoName      = x.InspeccionTipoName,
                                Orden                   = x.Orden,
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

            string idCategoria      = Globals.ParseGuid(data.idCategoria);
            string categoriaName    = Globals.ToUpper(data.name);

            // ENCONTRAR UNA CATEGORÍA PARA ACTUALIZAR
            Categoria objModel = await Find(idCategoria);

            if (objModel == null) { throw new ArgumentException("No se ha encontrado la categoría solicitada."); }
            if (objModel.Deleted) { throw new ArgumentException("La categoría ya fue eliminada anteriormente."); }

            bool isNameModified = !string.Equals(objModel.Name, categoriaName, StringComparison.OrdinalIgnoreCase);

            if (isNameModified)
            {
                bool findCategoriaName = await _context.Categorias.AnyAsync(x => x.Name.ToUpper() == categoriaName && x.IdCategoria != idCategoria && !x.Deleted);
                if (findCategoriaName) { throw new ArgumentException("Ya existe una categoría con ese nombre."); }
            }                        

            // ACTUALIZAR CATEGORÍA       
            objModel.Name = categoriaName;
            objModel.SetUpdated(Globals.GetUser(user));

            _context.Categorias.Update(objModel);

            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }
    }
}
