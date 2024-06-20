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
            string name = Globals.ToUpper(data.name);

            // Verificar si coincide el id de inspeccion tipo.
            var findInspeccionTipo = await _context.InspeccionesTipos.AnyAsync(x => x.IdInspeccionTipo == idInspeccionTipo && !x.Deleted);
            if (!findInspeccionTipo) { throw new ArgumentException("El tipo de inspección especificado no existe."); }

            // Obtener todas las categorias existentes para el tipo de inspeccion dado.
            var lstCategorias = await _context.Categorias
                                        .Where(x => x.IdInspeccionTipo == idInspeccionTipo && !x.Deleted)
                                        .OrderBy(x => x.Orden)
                                        .ToListAsync();

            // Determinar el nuevo valor de orden para la nueva categoría.
            int newOrdenValue = lstCategorias.Count > 0 ? lstCategorias.Max(x => x.Orden) + 1 : 1;

            if (lstCategorias.Any(x => x.Name.ToUpper() == name)) { throw new ArgumentException("Ya existe una categoría con ese nombre."); }

            // GUARDAR CATEGORIA
            Categoria objModel = new Categoria();

            objModel.IdCategoria            = Guid.NewGuid().ToString();
            objModel.Name                   = name;
            objModel.IdInspeccionTipo       = idInspeccionTipo;
            objModel.InspeccionTipoCodigo   = Globals.ToUpper(data.inspeccionTipoCodigo);
            objModel.InspeccionTipoName     = Globals.ToUpper(data.inspeccionTipoName);
            objModel.Orden                  = newOrdenValue;
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

            string idInspeccionTipo     = Globals.ParseGuid(data.idInspeccionTipo);
            string idCategoria          = Globals.ParseGuid(data.idCategoria);

            // ENCONTRAR CATEGORIA A ELIMINAR
            Categoria objModel = await Find(idCategoria);

            if (objModel == null) { throw new ArgumentException("No se encontró la categoría."); }
            if (objModel.Deleted) { throw new ArgumentException("La categoría ya fue eliminada anteriormente."); }

            // ELIMINAR CATEGORIA
            objModel.Deleted = true;
            objModel.Orden = 0;
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

        public async Task<List<dynamic>> ListEvaluacion(string idInspeccionTipo)
        {
            return await _context.Categorias
                            .AsNoTracking()
                            .Where(x => x.IdInspeccionTipo == idInspeccionTipo && !x.Deleted)
                            .OrderBy(x => x.Orden)
                            .Select(x => new
                            {
                                IdCategoria             = x.IdCategoria,
                                Name                    = x.Name,
                                TotalItems              = x.CategoriasItems.Where(d => !d.Deleted && d.IdFormularioTipo == "ea52bdfd-8af6-4f5a-b182-2b99e554eb32").Count(),
                                CategoriasItems         = x.CategoriasItems.Where(d => !d.Deleted)
                                                            .OrderBy(x => x.Orden)
                                                            .Select(d => new
                                                            {
                                                                IdCategoriaItem     = d.IdCategoriaItem,
                                                                Name                = d.Name,
                                                                IdFormularioTipo    = d.IdFormularioTipo,
                                                                FormularioTipoName  = d.FormularioTipoName,
                                                                FormularioValor     = d.FormularioValor,
                                                                NoAplica            = d.NoAplica,
                                                            }).ToList()
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

            string idInspeccionTipo     = Globals.ParseGuid(data.idInspeccionTipo);
            string idCategoria          = Globals.ParseGuid(data.idCategoria);

            string name = Globals.ToUpper(data.name);

            // ENCONTRAR CATEGORIA A ACTUALIZAR
            Categoria objModel = await Find(idCategoria);

            if (objModel == null) { throw new ArgumentException("No se encontró la categoría."); }
            if (objModel.Deleted) { throw new ArgumentException("La categoría ha sido eliminada."); }

            bool editName = !string.Equals(objModel.Name, name, StringComparison.OrdinalIgnoreCase);

            if (editName)
            {
                bool findName = await _context.Categorias.AnyAsync(x => x.Name.ToUpper() == name && x.IdCategoria != idCategoria && x.IdInspeccionTipo == idInspeccionTipo && !x.Deleted);
                if (findName) { throw new ArgumentException("Ya existe una categoría con ese nombre."); }
            }

            // ACTUALIZAR CATEGORIA
            objModel.Name = name;
            objModel.SetUpdated(Globals.GetUser(user));

            _context.Categorias.Update(objModel);
            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }
    }
}