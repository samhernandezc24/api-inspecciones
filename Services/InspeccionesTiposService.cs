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

            string codigo       = Globals.ToUpper(data.codigo);
            string name         = Globals.ToUpper(data.name);
            string displayName  = Globals.ToUpper($"Check List {data.name}");

            int lastOrdenValue  = await _context.InspeccionesTipos.MaxAsync(x => (int?)x.Orden) ?? 0;
            int newOrdenValue   = lastOrdenValue + 1;

            bool findCodigo     = await _context.InspeccionesTipos.AnyAsync(x => x.Codigo.ToUpper() == codigo && !x.Deleted);
            bool findName       = await _context.InspeccionesTipos.AnyAsync(x => x.Name.ToUpper() == name && !x.Deleted);

            if (findCodigo) { throw new ArgumentException("Ya existe un tipo de inspección con ese código."); }
            if (findName) { throw new ArgumentException("Ya existe un tipo de inspección con ese nombre."); }

            // GUARDAR TIPO DE INSPECCION
            InspeccionTipo objModel = new InspeccionTipo();

            objModel.IdInspeccionTipo   = Guid.NewGuid().ToString();
            objModel.Codigo             = codigo;
            objModel.Name               = name;
            objModel.DisplayName        = displayName;
            objModel.Orden              = newOrdenValue;
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

            // ENCONTRAR TIPO DE INSPECCION A ELIMINAR
            InspeccionTipo objModel = await Find(idInspeccionTipo);

            if (objModel == null) { throw new ArgumentException("No se encontró el tipo de inspección."); }
            if (objModel.Deleted) { throw new ArgumentException("El tipo de inspección ya fue eliminado anteriormente."); }

            // ELIMINAR TIPO DE INSPECCION
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

            string idInspeccionTipo = Globals.ParseGuid(data.idInspeccionTipo);

            string codigo       = Globals.ToUpper(data.codigo);
            string name         = Globals.ToUpper(data.name);
            string displayName  = Globals.ToUpper($"Check List {data.name}");

            // ENCONTRAR TIPO DE INSPECCION A ACTUALIZAR
            InspeccionTipo objModel = await Find(idInspeccionTipo);

            if (objModel == null) { throw new ArgumentException("No se encontró el tipo de inspección."); }
            if (objModel.Deleted) { throw new ArgumentException("El tipo de inspección ha sido eliminado."); }

            bool editCodigo = !string.Equals(objModel.Codigo, codigo, StringComparison.OrdinalIgnoreCase);
            bool editName   = !string.Equals(objModel.Name, name, StringComparison.OrdinalIgnoreCase);

            if (editCodigo || editName) 
            {
                bool findCodigo     = await _context.InspeccionesTipos.AnyAsync(x => x.Codigo.ToUpper() == codigo && x.IdInspeccionTipo != idInspeccionTipo && !x.Deleted);
                bool findName       = await _context.InspeccionesTipos.AnyAsync(x => x.Name.ToUpper() == name && x.IdInspeccionTipo != idInspeccionTipo && !x.Deleted);

                if (findCodigo) { throw new ArgumentException("Ya existe un tipo de inspección con ese código."); }
                if (findName) { throw new ArgumentException("Ya existe un tipo de inspección con ese nombre."); }            
            }            

            // ACTUALIZAR TIPO DE INSPECCION
            objModel.Codigo             = codigo;
            objModel.Name               = name;
            objModel.DisplayName        = displayName;
            objModel.SetUpdated(Globals.GetUser(user));

            _context.InspeccionesTipos.Update(objModel);
            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }
    }
}