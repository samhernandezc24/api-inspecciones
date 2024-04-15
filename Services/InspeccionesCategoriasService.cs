using API.Inspecciones.Interfaces;
using API.Inspecciones.Models;
using API.Inspecciones.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Workcube.Interfaces;
using Workcube.Libraries;

namespace API.Inspecciones.Services
{
    public class InspeccionesCategoriasService : IInspeccionesCategoriasService
    {
        private readonly Context _context;

        public InspeccionesCategoriasService(Context context)
        {
            _context = context;
        }

        public async Task Create(dynamic data, ClaimsPrincipal user)
        {
            if (!await HttpReq.GetPrivilegio("INSPECCIONES_CATEGORIAS_CREATE", user)) { throw new AppException(ExceptionMessage.SESSION_003); };

            var objUser         = Globals.GetUser(user);
            var objTransaction  = _context.Database.BeginTransaction();

            // GUARDAR CATEGORIA
            InspeccionCategoria objModel = new InspeccionCategoria();

            objModel.IdInspeccionCategoria  = Guid.NewGuid().ToString();
            objModel.Name                   = Globals.ToString(data.name);
            objModel.IdInspeccion           = Globals.ParseGuid(data.idInspeccion);
            objModel.InspeccionFolio        = Globals.ToString(data.inspeccionFolio);
            objModel.InspeccionName         = Globals.ToString(data.inspeccionName);
            objModel.SetCreated(objUser);

            _context.InspeccionesCategorias.Add(objModel);
            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public Task<dynamic> DataSource(dynamic data, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(dynamic data, ClaimsPrincipal user)
        {
            if (!await HttpReq.GetPrivilegio("INSPECCIONES_CATEGORIAS_DELETE", user)) { throw new AppException(ExceptionMessage.SESSION_003); }

            var objTransaction = _context.Database.BeginTransaction();

            // ELIMINAR CATEGORIA
            string idInspeccionCategoria = Globals.ParseGuid(data.idInspeccionCategoria);
            InspeccionCategoria objModel = await Find(idInspeccionCategoria);

            if (objModel.Deleted) { throw new ArgumentException("La categoría ya fue eliminada anteriormente"); }

            objModel.Deleted = true;
            objModel.SetUpdated(Globals.GetUser(user));

            _context.InspeccionesCategorias.Update(objModel);
            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public async Task<InspeccionCategoria> Find(string id)
        {
            return await _context.InspeccionesCategorias.FindAsync(id);
        }

        public async Task<InspeccionCategoria> FindSelectorById(string id, string fields)
        {
            return await _context.InspeccionesCategorias.AsNoTracking().Where(x => x.IdInspeccionCategoria == id)
                            .Select(Globals.BuildSelector<InspeccionCategoria, InspeccionCategoria>(fields)).FirstOrDefaultAsync();
        }

        public async Task<List<dynamic>> List()
        {
            return await _context.InspeccionesCategorias.AsNoTracking()
                            .Where(x => !x.Deleted)
                            .OrderBy(x => x.CreatedFecha)
                            .Select(x => new
                            {
                                IdInspeccionCategoria   = x.IdInspeccionCategoria,
                                Name                    = x.Name,
                            })
                            .ToListAsync<dynamic>();
        }

        public async Task<List<dynamic>> List(string idInspeccion)
        {
            return await _context.InspeccionesCategorias
                            .AsNoTracking()
                            .Where(x => x.IdInspeccion == idInspeccion && !x.Deleted)
                            .OrderBy(x => x.CreatedFecha)
                            .Select(x => new
                            {
                                IdInspeccionCategoria   = x.IdInspeccionCategoria,
                                Name                    = x.Name,
                            })
                            .ToListAsync<dynamic>();
                    
        }

        public Task<byte[]> Reporte(dynamic data)
        {
            throw new NotImplementedException();
        }

        public async Task Update(dynamic data, ClaimsPrincipal user)
        {
            if (!await HttpReq.GetPrivilegio("INSPECCIONES_CATEGORIAS_UPDATE", user)) { throw new AppException(ExceptionMessage.SESSION_003); };

            var objTransaction = _context.Database.BeginTransaction();

            // ACTUALIZAR CATEGORIA
            string idInspeccionCategoria = Globals.ParseGuid(data.idInspeccionCategoria);

            InspeccionCategoria objModel = await Find(idInspeccionCategoria);

            if (objModel == null) { throw new ArgumentException("No se ha encontrado la categoría."); }

            objModel.Name = Globals.ToString(data.name);

            _context.InspeccionesCategorias.Update(objModel);
            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }
    }
}
