using System.Security.Claims;
using API.Inspecciones.Models;
using API.Inspecciones.Persistence;
using Microsoft.EntityFrameworkCore;
using Workcube.Libraries;

namespace API.Inspecciones.Services
{
    public class InspeccionesFicherosService
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _root;

        public InspeccionesFicherosService(Context context, IWebHostEnvironment root)
        {
            _context    = context;
            _root       = root;
        }


        public async Task Create(dynamic data, ClaimsPrincipal user)
        {
            string fileBase64 = Globals.ToString(data.fileBase64);
            string fileExtension = "." + Globals.ToString(data.fileExtension);
            string filePath = FileManager.GetNamePath(fileExtension);

            var objTransaction = _context.Database.BeginTransaction();

            InspeccionFichero objModel = new InspeccionFichero();

            objModel.IdInspeccionFichero    = Guid.NewGuid().ToString();
            objModel.Path                   = filePath;
            objModel.IdInspeccion           = Globals.ParseGuid(data.idInspeccion);
            objModel.InspeccionFolio        = Globals.ToUpper(data.inspeccionFolio);
            objModel.SetCreated(Globals.GetUser(user));

            _context.InspeccionesFicheros.Add(objModel);
            await _context.SaveChangesAsync();

            // GUARDAR FICHERO
            if (!FileManager.ValidateExtension(fileExtension)) { throw new AppException(ExceptionMessage.CAST_002); }
            await HttpReq.SaveFile("\\Ficheros\\Mobile\\Inspecciones\\", filePath, fileBase64);

            // GUARDAR BASE DE DATOS
            objTransaction.Commit();
        }

        public async Task Delete(dynamic data, ClaimsPrincipal user)
        {
            var objTransaction = _context.Database.BeginTransaction();

            var idInspeccionFichero = Globals.ParseGuid(data.idInspeccionFichero);

            // ELIMINAR FOTOGRAFÍA
            InspeccionFichero objModel = await _context.InspeccionesFicheros.FindAsync(idInspeccionFichero);

            if (objModel == null) { throw new ArgumentException("No se ha encontrado la foto solicitada."); }
            if (objModel.Deleted) { throw new ArgumentException("La foto ya fue eliminado anteriormente."); }

            objModel.Deleted = true;
            objModel.SetUpdated(Globals.GetUser(user));

            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public async Task<List<dynamic>> List(string idInspeccion)
        {
            return await _context.InspeccionesFicheros
                            .AsNoTracking()
                            .Where(x => x.IdInspeccion == idInspeccion && !x.Deleted)
                            .OrderBy(x => x.CreatedFecha)
                            .Select(x => new
                            {
                                IdInspeccionFichero     = x.IdInspeccionFichero,
                                Path                    = x.Path,
                                CreatedUserName         = x.CreatedUserName,
                                CreatedFechaNatural     = x.CreatedFechaNatural,
                                UpdatedUserName         = x.UpdatedUserName,
                                UpdatedFechaNatural     = x.UpdatedFechaNatural,
                            })
                            .ToListAsync<dynamic>();
        }
    }
}
