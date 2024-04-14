using API.Inspecciones.Models;
using API.Inspecciones.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Workcube.Interfaces;
using Workcube.Libraries;

namespace API.Inspecciones.Services
{
    public class InspeccionesTiposService : IGlobal<InspeccionTipo>
    {
        private readonly Context _context;

        public InspeccionesTiposService(Context context)
        {
            _context = context;
        }

        public async Task Create(dynamic data, ClaimsPrincipal user)
        {
            var objTransaction = _context.Database.BeginTransaction();

            string inspeccionTipoDisplayName    = Globals.ToUpper($"Check List {data.name}");
            string inspeccionTipoCodigo         = Globals.ToUpper(data.codigo);
            string inspeccionTipoName           = Globals.ToUpper(data.name);

            int inspeccionTipoLastOrden         = await _context.InspeccionesTipos.MaxAsync(x => (int?)x.Orden) ?? 0;
            int inspeccionTipoNuevoOrden        = inspeccionTipoLastOrden + 1;

            bool findInspeccionTipoCodigo       = await _context.InspeccionesTipos.AnyAsync(x => x.Codigo.ToUpper() == inspeccionTipoCodigo && !x.Deleted);
            bool findInspeccionTipoName         = await _context.InspeccionesTipos.AnyAsync(x => x.Name.ToUpper() == inspeccionTipoName && !x.Deleted);

            if (findInspeccionTipoCodigo) { throw new ArgumentException("Ya existe un tipo de inspección con ese código."); }
            if (findInspeccionTipoName) { throw new ArgumentException("Ya existe un tipo de inspección con ese nombre."); }

            // GUARDAR TIPO DE INSPECCIÓN
            InspeccionTipo objModel = new InspeccionTipo();
            objModel.IdInspeccionTipo   = Guid.NewGuid().ToString();
            objModel.Codigo             = inspeccionTipoCodigo;
            objModel.Name               = inspeccionTipoName;
            objModel.DisplayName        = inspeccionTipoDisplayName;
            objModel.Orden              = inspeccionTipoNuevoOrden;
            objModel.SetCreated(Globals.GetUser(user));

            _context.InspeccionesTipos.Add(objModel);

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

            // ENCONTRAR UN TIPO DE INSPECCIÓN PARA ELIMINAR
            InspeccionTipo objModel = await Find(idInspeccionTipo);

            if (objModel == null) { throw new ArgumentException("No se ha encontrado el tipo de inspección solicitado."); }
            if (objModel.Deleted) { throw new ArgumentException("El tipo de inspección ya fue eliminado anteriormente."); }

            // ELIMINAR TIPO DE INSPECCIÓN
            objModel.Deleted = true;
            objModel.Orden = 0;
            objModel.SetUpdated(Globals.GetUser(user));

            _context.SaveChanges();

            var lstInspeccionesTipos = _context.InspeccionesTipos.OrderBy(x => x.Orden).Where(x => !x.Deleted).ToList();

            int orden = 1;
            foreach (var item in lstInspeccionesTipos)
            {
                item.Orden = orden;
                orden++;
            }

            _context.InspeccionesTipos.Update(objModel);

            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public async Task<InspeccionTipo> Find(string id)
        {
            return await _context.InspeccionesTipos.FindAsync(id);
        }

        public async Task<InspeccionTipo> FindSelectorById(string id, string fields)
        {
            return await _context.InspeccionesTipos.AsNoTracking().Where(x => x.IdInspeccionTipo == id)
                            .Select(Globals.BuildSelector<InspeccionTipo, InspeccionTipo>(fields)).FirstOrDefaultAsync();
        }

        public async Task<List<dynamic>> List()
        {
            return await _context.InspeccionesTipos
                            .AsNoTracking()
                            .Where(x => !x.Deleted)
                            .OrderBy(x => x.Orden)
                            .Select(x => new
                            {
                                IdInspeccionTipo    = x.IdInspeccionTipo,
                                Codigo              = x.Codigo,
                                Name                = x.Name,
                                Orden               = x.Orden,
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

            string idInspeccionTipo             = Globals.ParseGuid(data.idInspeccionTipo);

            string inspeccionTipoDisplayName    = Globals.ToUpper($"Check List {data.name}");
            string inspeccionTipoCodigo         = Globals.ToUpper(data.codigo);
            string inspeccionTipoName           = Globals.ToUpper(data.name);

            // ENCONTRAR UN TIPO DE INSPECCIÓN PARA ACTUALIZAR
            InspeccionTipo objModel = await Find(idInspeccionTipo);

            if (objModel == null) { throw new ArgumentException("No se ha encontrado el tipo de inspección solicitado."); }
            if (objModel.Deleted) { throw new ArgumentException("El tipo de inspección ya fue eliminado anteriormente."); }

            bool isCodigoModified   = !string.Equals(objModel.Codigo, inspeccionTipoCodigo, StringComparison.OrdinalIgnoreCase);
            bool isNameModified     = !string.Equals(objModel.Name, inspeccionTipoName, StringComparison.OrdinalIgnoreCase);

            if (isCodigoModified || isNameModified)
            {
                bool findInspeccionTipoCodigo   = await _context.InspeccionesTipos.AnyAsync(x => x.Codigo.ToUpper() == inspeccionTipoCodigo && x.IdInspeccionTipo != idInspeccionTipo && !x.Deleted);
                bool findInspeccionTipoName     = await _context.InspeccionesTipos.AnyAsync(x => x.Name.ToUpper() == inspeccionTipoName && x.IdInspeccionTipo != idInspeccionTipo && !x.Deleted);

                if (findInspeccionTipoCodigo) { throw new ArgumentException("Ya existe un tipo de inspección con ese código."); }
                if (findInspeccionTipoName) { throw new ArgumentException("Ya existe un tipo de inspección con ese nombre."); }
            }                        

            // ACTUALIZAR TIPO DE INSPECCIÓN            
            objModel.Codigo             = inspeccionTipoCodigo;
            objModel.Name               = inspeccionTipoName;
            objModel.DisplayName        = inspeccionTipoDisplayName;
            objModel.SetUpdated(Globals.GetUser(user));

            _context.InspeccionesTipos.Update(objModel);

            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }
    }
}
