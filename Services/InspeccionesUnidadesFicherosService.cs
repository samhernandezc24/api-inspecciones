using API.Inspecciones.Models;
using API.Inspecciones.Persistence;
using System.Data.Entity;
using System.Security.Claims;
using Workcube.Libraries;

namespace API.Inspecciones.Services
{
    public class InspeccionesUnidadesFicherosService
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _root;

        public InspeccionesUnidadesFicherosService(Context context, IWebHostEnvironment root)
        {
            _context = context;
            _root = root;

        }

        public async Task Create(dynamic data, ClaimsPrincipal user)
        {
            string fileBase64       = Globals.ToString(data.fileBase64);
            string fileExtension    = "." + Globals.ToString(data.fileExtension);
            string filePath         = FileManager.GetNamePath(fileExtension);

            var objTransaction = _context.Database.BeginTransaction();

            InspeccionUnidadFichero objModel = new InspeccionUnidadFichero();

            objModel.IdInspeccionUnidadFichero  = Guid.NewGuid().ToString();
            objModel.Path                       = filePath;
            objModel.IdInspeccionUnidad         = Globals.ParseGuid(data.idInspeccionUnidad);
            objModel.InspeccionUnidadFolio      = Globals.ToString(data.inspeccionUnidadFolio);
            objModel.Orden                      = 0;
            objModel.SetCreated(Globals.GetUser(user));

            _context.InspeccionesUnidadesFicheros.Add(objModel);
            await _context.SaveChangesAsync();

            string directory = _root.ContentRootPath + "\\Ficheros\\InspeccionesUnidadesFicheros\\";

            if (!FileManager.ValidateExtension(fileExtension)) { throw new AppException(ExceptionMessage.CAST_002); }

            FileManager.ValidateDirectory(directory);

            await FileManager.SaveFileBase64(fileBase64, directory + objModel.Path);

            objTransaction.Commit();
        }     

        public async Task Delete(dynamic data, ClaimsPrincipal user)
        {
            var objTransaction = _context.Database.BeginTransaction();

            var idInspeccionUnidadFichero = Globals.ParseGuid(data.idInspeccionUnidadFichero);

            InspeccionUnidadFichero objModel = await _context.InspeccionesUnidadesFicheros.FindAsync(idInspeccionUnidadFichero);

            if (objModel.Deleted) { throw new ArgumentException("La foto ya ha había sido eliminada anteriormente"); }

            objModel.Deleted = true;
            objModel.SetUpdated(Globals.GetUser(user));

            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }
        
        public async Task<dynamic> List(string idInspeccionUnidad)
        {
            var lstInspeccionesUnidadesFicheros = await _context.InspeccionesUnidadesFicheros
                                                            .AsNoTracking()
                                                            .Where(x => x.IdInspeccionUnidad == idInspeccionUnidad && !x.Deleted)
                                                            .OrderBy(x => x.CreatedFecha)
                                                            .Select(x => new
                                                            {
                                                                IdInspeccionUnidadFichero   = x.IdInspeccionUnidadFichero,
                                                                Path                        = x.Path,
                                                                CreatedUserName             = x.CreatedUserName,
                                                                CreatedFechaNatural         = x.CreatedFechaNatural,
                                                                UpdatedUserName             = x.UpdatedUserName,
                                                                UpdatedFechaNatural         = x.UpdatedFechaNatural
                                                            })
                                                            .ToListAsync();

            return lstInspeccionesUnidadesFicheros;
        }

        public async Task Update(dynamic data, ClaimsPrincipal user)
        {
            var objTransaction = _context.Database.BeginTransaction();

            var idInspeccionUnidadFichero = Globals.ParseGuid(data.idInspeccionUnidadFichero);

            InspeccionUnidadFichero objModel = await _context.InspeccionesUnidadesFicheros.FindAsync(idInspeccionUnidadFichero);

            objModel.SetUpdated(Globals.GetUser(user));

            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }
    }
}
