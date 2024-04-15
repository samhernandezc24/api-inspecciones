using API.Inspecciones.Interfaces;
using API.Inspecciones.Models;
using API.Inspecciones.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Workcube.Libraries;

namespace API.Inspecciones.Services
{
    public class InspeccionesCategoriasItemsService : IInspeccionesCategoriasItemsService
    {
        private readonly Context _context;

        public InspeccionesCategoriasItemsService(Context context)
        {
            _context = context;
        }
        public async Task Create(dynamic data, ClaimsPrincipal user)
        {
            if (!await HttpReq.GetPrivilegio("INSPECCIONES_CATEGORIAS_ITEMS_CREATE", user)) { throw new AppException(ExceptionMessage.SESSION_003); };

            var objTransaction = _context.Database.BeginTransaction();

            InspeccionCategoriaItem objModel = new InspeccionCategoriaItem();

            objModel.IdInspeccionCategoriaItem  = Guid.NewGuid().ToString();
            objModel.Name                       = Globals.ToString(data.name);
            objModel.FormularioTipo             = Globals.ToString(data.formularioTipo);
            objModel.FormularioValor            = Globals.ToString(data.formularioValor);
            objModel.IdInspeccion               = Globals.ParseGuid(data.idInspeccion);
            objModel.InspeccionName             = Globals.ToString(data.inspeccionName);
            objModel.IdInspeccionCategoria      = Globals.ParseGuid(data.idInspeccionCategoria);
            objModel.InspeccionCategoriaName    = Globals.ToString(data.inspeccionCategoriaName);

            _context.InspeccionesCategoriasItems.Add(objModel);
            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public Task<dynamic> DataSource(dynamic data, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task Delete(dynamic data, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public async Task<InspeccionCategoriaItem> Find(string id)
        {
            return await _context.InspeccionesCategoriasItems.FindAsync(id);
        }

        public async Task<InspeccionCategoriaItem> FindSelectorById(string id, string fields)
        {
            return await _context.InspeccionesCategoriasItems.Where(x => x.IdInspeccionCategoriaItem == id).Select(Globals.BuildSelector<InspeccionCategoriaItem, InspeccionCategoriaItem>(fields)).FirstOrDefaultAsync();
        }

        public async Task<List<dynamic>> List()
        {
            return await _context.InspeccionesCategoriasItems.AsNoTracking()
                            .Where(x => !x.Deleted)
                            .OrderBy(x => x.CreatedFecha)
                            .Select(x => new
                            {
                                IdInspeccionCategoriaItem   = x.IdInspeccionCategoriaItem,
                                Name                        = x.Name,
                                FormularioTipo              = x.FormularioTipo,
                                FormularioValor             = x.FormularioValor,
                            })
                            .ToListAsync<dynamic>();
        }

        public Task<byte[]> Reporte(dynamic data)
        {
            throw new NotImplementedException();
        }

        public async Task Update(dynamic data, ClaimsPrincipal user)
        {
            if (!await HttpReq.GetPrivilegio("INSPECCIONES_CATEGORIAS_ITEMS_UPDATE", user)) { throw new AppException(ExceptionMessage.SESSION_003); };

            var objTransaction = _context.Database.BeginTransaction();

            // ACTUALIZAR FORMULARIO
            string idInspeccionCategoriaItem = Globals.ParseGuid(data.idInspeccionCategoriaItem);

            InspeccionCategoriaItem objModel = await Find(idInspeccionCategoriaItem);

            if (objModel == null) { throw new ArgumentException("No se ha encontrado el formulario de control."); }

            objModel.Name               = Globals.ToString(data.name);
            objModel.FormularioTipo     = Globals.ToString(data.formularioTipo);
            objModel.FormularioValor    = Globals.ToString(data.formularioValor);

            _context.InspeccionesCategoriasItems.Update(objModel);
            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }
    }
}
